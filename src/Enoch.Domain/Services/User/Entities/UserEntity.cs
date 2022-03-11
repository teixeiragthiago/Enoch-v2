using Enoch.Domain.Services.User.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Enoch.Domain.Services.User.Entities
{
    [Table("User")]
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserEnum.Profile Profile { get; set; }
        public UserEnum.Status Status { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateRegister { get; set; }
    }
}
