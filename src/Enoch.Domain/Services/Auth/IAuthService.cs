namespace Enoch.Domain.Services.Auth
{
    public interface IAuthService
    {
        bool VerifyUser(int idUser, string token);
    }
}
