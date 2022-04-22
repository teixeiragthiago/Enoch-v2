using Enoch.Domain.Services.Auth.Dto;
using Enoch.Domain.Services.User.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixtures.Auth
{
    public class AuthFixtureDto
    {
        public static UserTokenDto CreateValidToken()
        {
            return new UserTokenDto
            {
                Name = "THIAGO",
                Email = "thiago@cloudmed.io",
                Profile = UserEnum.Profile.Adminstrator,
                Token = "TOKEN TESTE"
            };
        }

        public static UserTokenDto CreateInvalidToken()
        {
            return new UserTokenDto
            {
                Name = "",
                Email = "",
                Profile = UserEnum.Profile.Adminstrator,
                Token = ""
            };
        }

        public static AuthDto CreateAuthData()
        {
            return new AuthDto
            {
                Email = "thiago@cloudmed.io",
                Password = "AS67xOi5#"
            };
        }

        public static AuthDto CreateInvalidAuthData()
        {
            return new AuthDto
            {
                Email = "",
                Password = ""
            };
        }
    }
}
