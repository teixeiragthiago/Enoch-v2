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

        public string FormatYearRegister()
        {
            var currentYear = DateTime.Now.Year;
            if (DateRegister.Year == currentYear)
                return "O Usuário foi cadastrado no ano atual!";
            else if (DateRegister.Year < currentYear)
            {
                return $"O Usuário foi cadastrado no anterior! {DateRegister.Year}";
            }
            else
                return "O Usuário ainda não foi cadastrado!";
        }
    }

    public class Pedido
    {
        public int Id { get; set; }
        public int Id_Produto { get; set; }
        public int Quantidade { get; set; }
        public float Preco { get; set; }
        

        public float ValorTotal()
        {
            return Quantidade * Preco;
        }
    }

}
