using Enoch.Domain.Services.User.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoch.Domain.Services.User.Queue
{
    public interface IUserQueue
    {
        bool SendQueue(UserEntity user);

    }
}
