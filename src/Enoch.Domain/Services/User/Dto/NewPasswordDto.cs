using Enoch.CrossCutting.Notification;

namespace Enoch.Domain.Services.User.Dto
{
    public class NewPasswordDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsValidInsert(INotification _notification)
        {
            if (string.IsNullOrEmpty(Password))
                return _notification.AddWithReturn<bool>("Para realizar a alteração de senha, você precisa informar uma senha válida!");

            return true;
        }
    }
}
