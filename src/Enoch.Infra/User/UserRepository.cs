using Enoch.Domain.Services.User;
using Enoch.Domain.Services.User.Entities;
using Enoch.Infra.Base;
using Enoch.Infra.Context;
using System.Linq;

namespace Enoch.Infra.User
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        public void PutPassword(int idUser, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var context = new DataContext())
            {
                var user = context.User.FirstOrDefault(x => x.Id == idUser);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                context.User.Update(user);

                context.SaveChanges();
            }
        }
    }
}
