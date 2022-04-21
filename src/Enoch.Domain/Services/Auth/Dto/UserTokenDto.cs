using Enoch.Domain.Services.User.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoch.Domain.Services.Auth.Dto
{
    public class UserTokenDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public UserEnum.Profile Profile { get; set; }
        public string ProfileName => Profile.ToString();
        public string Token { get; set; }
    }
}
