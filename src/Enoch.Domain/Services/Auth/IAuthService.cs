using Enoch.Domain.Services.Auth.Dto;
using Enoch.Domain.Services.User.Dto;

namespace Enoch.Domain.Services.Auth
{
    public interface IAuthService
    {
        UserDataDto VerifyUser(string email, string password);
        UserTokenDto GenerateToken(AuthDto authDto);
    }
}
