using Enoch.Domain.Base;
using Enoch.Domain.Services.User.Entities;

namespace Enoch.Domain.Services.User
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        void PutPassword(int idUser, byte[] passwordHash, byte[] passwordSalt);
    }
}
