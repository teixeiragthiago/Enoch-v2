using Enoch.CrossCutting;
using Enoch.CrossCutting.Notification;
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

        public bool IsValid(INotification notification)
        {
            if (string.IsNullOrEmpty(Email) || !Email.IsValidMail())
                return notification.AddWithReturn<bool>("Ops.. o e-mail informado não é válido!");

            return true;
        }
    }
}
