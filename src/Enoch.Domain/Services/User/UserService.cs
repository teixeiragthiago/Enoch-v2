using Enoch.CrossCutting;
using Enoch.CrossCutting.AwsS3;
using Enoch.CrossCutting.Notification;
using Enoch.Domain.Services.User.Common;
using Enoch.Domain.Services.User.Dto;
using Enoch.Domain.Services.User.Entities;
using Enoch.Domain.Services.User.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Enoch.Domain.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotification _notification;
        private readonly IUserFactory _userFactory;
        private readonly IUserQueue _userQueue;
        private readonly AwsStorage _awsS3;
        private readonly string _bucketName = Environment.GetEnvironmentVariable("AWS_BUCKET");

        public UserService(IUserRepository userRepository, INotification notification, IUserFactory userFactory, IUserQueue userQueue, AwsStorage awsS3)
        {
            _userRepository = userRepository;
            _notification = notification;
            _userFactory = userFactory;
            _userQueue = userQueue;
            _awsS3 = awsS3;
        }

        public IEnumerable<UserDataDto> Get(out int total, int? page = null)
        {
            var users = _userRepository.Get(out total, page);

            return users.Select(x => new UserDataDto
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                DateRegister = x.DateRegister,
                Profile = x.Profile,
                Status = x.Status,
                Image = GetUserImage(x.ImagePath)
            });
        }
        public UserDataDto GetById(int id)
        {
            var user = _userRepository.First(x => x.Id == id);
            if (user == null) return null;

            return new UserDataDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                DateRegister = user.DateRegister,
                Profile = user.Profile,
                Status = user.Status,
                Image = GetUserImage(user.ImagePath)
            };
        }

        public int Post(UserDto user)
        {
            if (!user.IsValid(_notification))
                return _notification.AddWithReturn<int>(_notification.GetNotifications());

            if (!_userFactory.VerifyPassword(user.Password))
                return _notification.AddWithReturn<int>(_notification.GetNotifications());

            Encryption.CreatePasswordHash(user.Password, out var passwordHash, out var passwordSalt);

            var userEntity = new UserEntity
            {
                Name = user.Name,
                Email = user.Email,
                Status = UserEnum.Status.Enabled,
                Profile = user.Profile,
                DateRegister = DateTime.Now,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            int idUser = 0;
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var existingEmail = _userRepository.First(x => x.Email.LowerAndTrim() == user.Email.LowerAndTrim());
                if (existingEmail != null)
                    return _notification.AddWithReturn<int>("Ops.. parece que o e-mail informado já possui cadastro!");

                idUser = _userRepository.Post(userEntity);


                transaction.Complete();
            }

            //_userQueue.SendQueue(userEntity);

            _userQueue.SendMessageQueue(userEntity);

            return idUser;
        }

        public bool Put(UserDto user)
        {
            if (!user.IsValidUpdate(_notification))
                return _notification.AddWithReturn<bool>(_notification.GetNotifications());

            using (var transaction = new TransactionScope())
            {
                var userData = _userRepository.First(x => x.Id == user.Id);
                if (userData == null)
                    return _notification.AddWithReturn<bool>("Ops.. o usuário informado não pode ser encontrado!");

                if (userData.Email.ToLower().Trim() != user.Email.ToLower().Trim())
                {
                    var userEmail = _userRepository.First(x => x.Email.ToLower().Trim() == user.Email.ToLower().Trim());
                    if (userEmail != null)
                        return _notification.AddWithReturn<bool>("Ops.. o e-mail informado já possui cadastro!");
                }

                userData.Name = user.Name;
                userData.Email = user.Email;
                userData.Status = user.Status;
                userData.Profile = user.Profile;

                _userRepository.Put(userData);

                transaction.Complete();
            }

            return true;
        }
        public bool Delete(int id)
        {
            var user = _userRepository.First(x => x.Id == id);
            if (user == null)
                return _notification.AddWithReturn<bool>("Ops.. parece que o usuário informado não pode ser encontrado!");

            _userRepository.Delete(user);

            _ = _awsS3.Delete(user.ImagePath, _bucketName);

            return true;

        }

        public bool PutPassword(NewPasswordDto password)
        {
            if (password == null)
                return _notification.AddWithReturn<bool>("Ops.. acho que você não informou dados para realizar a alteração de senha.. :( ");

            if (!password.IsValidInsert(_notification))
                return _notification.AddWithReturn<bool>(_notification.GetNotifications());

            var user = _userRepository.First(x => x.Email.LowerAndTrim() == password.Email.LowerAndTrim());
            if (user == null)
                return _notification.AddWithReturn<bool>("Parece que o usuário informado não pode ser encontrado... Poderia confirmar?");

            if (!_userFactory.VerifyPassword(password.Password))
                return false;

            Encryption.CreatePasswordHash(password.Password, out var passwordHash, out var passwordSalt);

            _userRepository.PutPassword(user.Id, passwordHash, passwordSalt);

            return true;
        }

        public async Task<bool> UploadFile(string file, string key)
        {
            byte[] fileByte = file.CastBase64();

            var bucketName = Environment.GetEnvironmentVariable("AWS_BUCKET");
            if (string.IsNullOrEmpty(bucketName))
                return _notification.AddWithReturn<bool>("Ops.. parece que o Bucket não foi informado!");

            await _awsS3.UploadFileAsync(fileByte, key, bucketName);

            return true;
        }

        private string GetUserImage(string keyName)
        {
            if (string.IsNullOrEmpty(_bucketName))
                return string.Empty;

            var url = _awsS3.UrlFile(keyName, _bucketName);

            return string.IsNullOrEmpty(url) ? string.Empty : url;
        }

    }
}
