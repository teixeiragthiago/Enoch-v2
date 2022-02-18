using Enoch.CrossCutting.Notification;
using Enoch.Domain.Services.User;

namespace Enoch.Domain.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotification _notification;

        public AuthService(IUserRepository userRepository, INotification notification)
        {
            _userRepository = userRepository;
            _notification = notification;
        }

        public bool VerifyUser(int idUser, string token)
        {
            var user = _userRepository.First(x => x.Id == idUser);
            if (user == null)
                return false;

            return true;
        }
    }
}
