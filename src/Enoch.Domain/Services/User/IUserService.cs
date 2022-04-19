using Enoch.Domain.Services.User.Dto;
using System.Collections.Generic;

namespace Enoch.Domain.Services.User
{
    public interface IUserService
    {
        IEnumerable<UserDataDto> Get(out int total, int? page = null);
        UserDataDto GetById(int id);
        int Post(UserDto user);
        bool Put(UserDto user);
        bool Delete(int id);
        bool PutPassword(NewPasswordDto password);
        bool PostMessage(UserDto user);
    }
}
