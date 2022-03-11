using Enoch.CrossCutting;
using Enoch.CrossCutting.Notification;
using Enoch.Domain.Services.User.Common;
using System;

namespace Enoch.Domain.Services.User.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserEnum.Profile Profile { get; set; }
        public UserEnum.Status Status { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }
        public string ImageFormat { get; set; }
        public DateTime DateRegister { get; set; }

        public bool IsValid(INotification notification)
        {
            if (string.IsNullOrEmpty(Name))
                return notification.AddWithReturn<bool>("Ops.. o nome informado não é válido!");

            if (string.IsNullOrEmpty(Email) || !Email.IsValidMail())
                return notification.AddWithReturn<bool>("Ops.. o e-mail informado não é válido!");

            if(Profile <= 0)
                return notification.AddWithReturn<bool>("Ops.. o perfil de usuário informado não é válido!");

            if(string.IsNullOrEmpty(Password))
                return notification.AddWithReturn<bool>("Ops.. a senha informada não é válida!");

            return true;
        }

        public bool IsValidUpdate(INotification notification)
        {
            if (Id <= 0)
                return notification.AddWithReturn<bool>("Ops.. o usuário informado não é válido!");

            if (string.IsNullOrEmpty(Name))
                return notification.AddWithReturn<bool>("Ops.. o nome informado não é válido!");

            if (string.IsNullOrEmpty(Email) || !Email.IsValidMail())
                return notification.AddWithReturn<bool>("Ops.. o e-mail informado não é válido!");

            if(Profile <= 0)
                return notification.AddWithReturn<bool>("Ops.. o perfil de usuário informado não é válido!");

            if (Status <= 0)
                return notification.AddWithReturn<bool>("Ops.. o status informado não é válido!");

            return true;
        }
    }
}
