using Enoch.Domain.Services.User.Common;
using Enoch.Domain.Services.User.Dto;
using System;
using System.Collections.Generic;

namespace Fixtures.User
{
    public static class UserFixtureDto
    {
        public static UserDto CreateValidAdmin()
        {
            return new UserDto
            {
                Id = 1,
                Name = "THIAGO",
                Email = "thiago@cloudmed.io",
                Status = UserEnum.Status.Enabled,
                Profile = UserEnum.Profile.Adminstrator,
                Password = "abR3Ir#l0101@",
                ImageFormat = string.Empty,
                Image = string.Empty
            };
        }

        public static UserDto CreateValidAdmin(string name, string email)
        {
            var random = new Random();

            return new UserDto
            {
                Id = random.Next(1, 10),
                Name = name,
                Email = email,
                Status = UserEnum.Status.Enabled,
                Profile = UserEnum.Profile.Adminstrator,
                Password = "Thiago123",
                ImageFormat = "jpg",
                Image = "imagem"
            };
        }

        public static UserDto CreateInvalidAdmin()
        {
            return new UserDto
            {
                Id = 0,
                Name = "",
                Email = "thiago.net",
                Status = UserEnum.Status.Enabled,
                Profile = UserEnum.Profile.Adminstrator,
                Password = "",
                ImageFormat = "",
                Image = ""
            };
        }

        public static UserDto CreateValidUser()
        {
            return new UserDto
            {
                Id = 1,
                Name = "THIAGO",
                Email = "thiago@cloudmed.io",
                Status = UserEnum.Status.Enabled,
                Profile = UserEnum.Profile.User,
                Password = "Thiago123",
                ImageFormat = "jpg",
                Image = "imagem"
            };
        }

        public static UserDto CreateInvalidUser()
        {
            return new UserDto
            {
                Id = 1,
                Name = "",
                Email = "thiago.net",
                Status = UserEnum.Status.Enabled,
                Profile = UserEnum.Profile.User,
                Password = ""
            };
        }

        public static IEnumerable<UserDto> CreateListValidUsers()
        {
            var users = new List<UserDto>();

            users.Add(CreateValidAdmin("Lucas", "lucas@gmail.io"));
            users.Add(CreateValidAdmin("Pedro", "pedro@gmail.io"));
            users.Add(CreateValidAdmin("Marina", "marina@gmail.io"));
            users.Add(CreateValidAdmin("Caio", "caio@gmail.io"));
            users.Add(CreateValidAdmin("Thiago", "thiago@gmail.io"));
            users.Add(CreateValidAdmin("Erica", "erica@gmail.io"));
            users.Add(CreateValidAdmin("Luciano", "luciano@gmail.io"));

            return users;
        }
    }
}
