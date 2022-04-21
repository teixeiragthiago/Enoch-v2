using Enoch.Domain.Services.User.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enoch.Domain.Services.Auth.Dto
{
    public class AuthDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
