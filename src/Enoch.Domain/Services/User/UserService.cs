using Enoch.CrossCutting;
using Enoch.CrossCutting.Notification;
using Enoch.Domain.Services.User.Common;
using Enoch.Domain.Services.User.Dto;
using Enoch.Domain.Services.User.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Enoch.Domain.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotification _notification;
        private readonly IUserFactory _userFactory;


        public UserService(IUserRepository userRepository, INotification notification, IUserFactory userFactory)
        {
            _userRepository = userRepository;
            _notification = notification;
            _userFactory = userFactory;
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
                Status = x.Status
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
                Status = user.Status
            };
        }

        public int Post(UserDto user)
        {
            if (!user.IsValid(_notification))
                return _notification.AddWithReturn<int>(_notification.GetNotifications());

            using (var  transaction = new TransactionScope())
            {
                var existingEmail = _userRepository.First(x => x.Email.LowerAndTrim() == user.Email.LowerAndTrim());
                if (existingEmail != null)
                    return _notification.AddWithReturn<int>("Ops.. parece que o e-mail informado já possui cadastro!");

                if (!_userFactory.VerifyPassword(user.Password))
                    return _notification.AddWithReturn<int>(_notification.GetNotifications());

                Encryption.CreatePasswordHash(user.Password, out var passwordHash, out var passwordSalt);

                var idUser = _userRepository.Post(new UserEntity
                {
                    Name = user.Name,
                    Email = user.Email,
                    Status = UserEnum.Status.Enabled,
                    Profile = user.Profile,
                    DateRegister = DateTime.Now,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                });

                transaction.Complete();

                return idUser;
            }
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

                if(userData.Email.ToLower().Trim() != user.Email.ToLower().Trim())
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

        //public int Post(UserRegisterDto entity)
        //{
        //    if (entity == null)
        //        return _notification.AddWithReturn<int>("Dados não informados para realizar cadastro");

        //    if (!entity.isValid(_notification))
        //        return default(int);

        //    var user = _userRepository.GetByEmail(entity.Email);
        //    if (user != null)
        //        return _notification.AddWithReturn<int>("E-mail já possui cadastro no sistema");

        //    var program = _programsRepository.GetByName(entity.Software);
        //    if (program == null)
        //        return _notification.AddWithReturn<int>("Programa informado para cadastro não encontrado!");

        //    var code = GenerateCode(Guid.NewGuid().ToString());
        //    var dateExec = DateTime.Now;

        //    byte[] Image = entity.Image.CastBase64();

        //    var idUser = _userRepository.Post(new UserEntity
        //    {
        //        Name = entity.Name,
        //        Email = entity.Email,
        //        Telephone = FormatTelephone(entity.Telephone),
        //        DateRegister = dateExec,
        //        Image = Image,
        //        PendingConfirm = true,
        //        Key = code,
        //        ExpirationKey = dateExec.AddMinutes(30)
        //    });

        //    _userProgramsRepository.Post(new UserProgramsEntity
        //    {
        //        Id_User = idUser,
        //        Id_Program = program.Id,
        //        Blocked = false
        //    });

        //    CreateLog($"Usuário foi cadastro e vinculado ao sistema: {program.Name}", idUser);

        //    var email = _emailFactory.Register(entity.Software, entity.Name, $"{program.Url}/confirm/{code}");

        //    _email.Send(_parameters.Data.Mail.From, _parameters.Data.Mail.PassWord, entity.Email,
        //        $"Confirmação de cadastro no {program.Name}", email);

        //    return idUser;
        //}

    }
}
