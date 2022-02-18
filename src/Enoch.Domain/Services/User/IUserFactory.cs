namespace Enoch.Domain.Services.User
{
    public interface IUserFactory
    {
         bool VerifyPassword(string password);
    }
}