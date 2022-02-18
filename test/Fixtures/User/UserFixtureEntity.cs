using Enoch.Domain.Services.User.Common;
using Enoch.Domain.Services.User.Entities;
using System;
using System.Collections.Generic;

namespace Fixtures.User
{
    public static class UserFixtureEntity
    {
        public static UserEntity CreateValidAdmin()
        {
            return new UserEntity
            {
                Id = 1,
                Name = "THIAGO",
                Email = "thiago@cloudmed.io",
                Status = UserEnum.Status.Enabled,
                Profile = UserEnum.Profile.Adminstrator,
            };
        }

        public static UserEntity CreateValidAdmin(string name, string email)
        {
            var random = new Random();

            return new UserEntity
            {
                Id = random.Next(1, 10),
                Name = name,
                Email = email,
                Status = UserEnum.Status.Enabled,
                Profile = UserEnum.Profile.Adminstrator,
            };
        }

        public static UserEntity CreateInvalidAdmin()
        {
            return new UserEntity
            {
                Id = 1,
                Name = "",
                Email = "thiago.net",
                Status = UserEnum.Status.Enabled,
                Profile = UserEnum.Profile.Adminstrator,
            };
        }

        public static UserEntity CreateValidUser()
        {
            return new UserEntity
            {
                Id = 1,
                Name = "THIAGO",
                Email = "thiago@cloudmed.io",
                Status = UserEnum.Status.Enabled,
                Profile = UserEnum.Profile.User,
            };
        }

        public static UserEntity CreateValidUser(string name, string email)
        {
            var random = new Random();

            return new UserEntity
            {
                Id = random.Next(1, 10),
                Name = name,
                Email = email,
                Status = UserEnum.Status.Enabled,
                Profile = UserEnum.Profile.Adminstrator,
            };
        }

        public static UserEntity CreateInvalidUser()
        {
            return new UserEntity
            {
                Id = 1,
                Name = "",
                Email = "thiago.net",
                Status = UserEnum.Status.Enabled,
                Profile = UserEnum.Profile.User,
            };
        }

        public static IEnumerable<UserEntity> CreateListValidUsers()
        {
            var users = new List<UserEntity>();

            users.Add(CreateValidUser("Lucas", "lucas@gmail.io"));
            users.Add(CreateValidAdmin("Pedro", "pedro@gmail.io"));
            users.Add(CreateValidUser("Marina", "marina@gmail.io"));
            users.Add(CreateValidAdmin("Caio", "caio@gmail.io"));
            users.Add(CreateValidAdmin("Thiago", "thiago@gmail.io"));
            users.Add(CreateValidUser("Erica", "erica@gmail.io"));
            users.Add(CreateValidUser("Luciano", "luciano@gmail.io"));
            users.Add(CreateValidAdmin("Águeda", "agueda@gmail.io"));

            return users;
        }
    }
}
