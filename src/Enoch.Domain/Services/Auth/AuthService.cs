using Enoch.CrossCutting;
using Enoch.CrossCutting.Notification;
using Enoch.Domain.Services.Auth.Dto;
using Enoch.Domain.Services.User;
using Enoch.Domain.Services.User.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Enoch.Domain.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotification _notification;
        private readonly IConfiguration _configuration;
        private readonly IUserFactory _userFactory;

        public AuthService(IUserRepository userRepository, INotification notification, IConfiguration configuration, IUserFactory userFactory)
        {
            _userRepository = userRepository;
            _notification = notification;
            _configuration = configuration;
            _userFactory = userFactory;
        }

        public UserTokenDto GenerateToken(AuthDto dto)
        {
            var user = VerifyUser(dto.Email, dto.Password);
            if (user == null)
                return _notification.AddWithReturn<UserTokenDto>(_notification.GetNotifications());

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.Get<AppSettings>().Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Profile.ToString())
                }) 
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new UserTokenDto
            {
                Name = user.Name,
                Email = user.Email,
                Profile = user.Profile,
                Token = tokenHandler.WriteToken(token),
            };
        }

        public UserDataDto VerifyUser(string email, string password)
        {
            var user = _userRepository.First(x => x.Email.LowerAndTrim() == email.LowerAndTrim());
            if (user == null)
                return _notification.AddWithReturn<UserDataDto>("Ops.. não foi possível encontrar o usuário!");

            var verifyPassword = Encryption.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt);
            if(!verifyPassword)
                return _notification.AddWithReturn<UserDataDto>("Ops.. a senha informada não está correta!");

            return new UserDataDto
            {
                Email = user.Email,
                Name = user.Name,
                Profile = user.Profile,
                Status = user.Status
            };
        }
    }
}
