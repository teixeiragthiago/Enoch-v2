using Enoch.Domain.Services.User.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enoch.Domain.Services.User.Dto
{
    public class UserDataDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserEnum.Profile Profile { get; set; }
        public UserEnum.Status Status { get; set; }
        public DateTime DateRegister { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public string ProfileName => Profile.ToString();
    }
}
