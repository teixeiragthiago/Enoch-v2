using Enoch.CrossCutting.LogWriter;
using Enoch.CrossCutting.Notification;
using Enoch.CrossCutting.RabbitMQConfig;
using Enoch.Domain.Services.User.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Enoch.Domain.Services.User
{
    public class UserFactory : IUserFactory
    {
        private readonly INotification _notification;
        private IConnection _connection;
        private readonly string _queueName = RabbitMQConfig.GetData().QueueName;

        public UserFactory(INotification notification)
        {
            _notification = notification;
        }

        public bool VerifyPassword(string password)
        {
            const string baseMessage = "Ops... Encotramos um problema para criar sua senha, pensamos em sua segurança";

            password = password.Trim();
            if (password.Length < 8)
                return _notification.AddWithReturn<bool>($"{baseMessage}, sua senha deve ter mais de 8 carateres!.");

            if (string.IsNullOrWhiteSpace(password))
                return _notification.AddWithReturn<bool>($"{baseMessage}, sua senha não deve possui espaços em branco!.");

            if (!VerifyUpperCaseLetters(password))
                return _notification.AddWithReturn<bool>($"{baseMessage}, precisamos ao menos um caractere maiúsculo .");

            if (!VerifyLowerCaseLetters(password))
                return _notification.AddWithReturn<bool>($"{baseMessage}, precisamos ao menos um caractere minúsculo.");

            if (VerifySequencePassword(password))
                return _notification.AddWithReturn<bool>($"{baseMessage}, não utilizar letras sequencias ex: ABCD, ZYXW, 1234 ou 9876.");

            if (VerifySequencePassword(password, true))
                return _notification.AddWithReturn<bool>($"{baseMessage}, não utilizar letras sequencias ex: ABCD, ZYXW, 1234 ou 9876.");

            if (!VerifyIsNumbers(password))
                return _notification.AddWithReturn<bool>($"{baseMessage}, sua senha precisa conter ao menos um número.");

            if (!VerifyIsLetters(password))
                return _notification.AddWithReturn<bool>($"{baseMessage}, sua senha precisa conter no minimo 2 letras.");

            return true;
        }

        private bool VerifySequencePassword(string password, bool reverse = false)
        {
            password = password.ToLower();
            var stringPatterm = @"(abc|bcd|cde|def|efg|fgh|ghi|hij|ijk|jkl|klm|lmn|mno|nop|opq|pqr|qrs|rst|stu|tuv|uvw|vwx|wxy|xyz|012|123|234|345|456|567|678|789)";

            if (reverse)
                stringPatterm = @"(987|876|765|654|543|432|321|210|zyx|yxw|xwv|wvu|vut|uts|tsr|srq|rqp|qpo|pon|onm|nml|mlk|lkj|kji|jih|ihg|hgf|gfe|fed|edc|dcb|cba)";

            return new Regex(stringPatterm).IsMatch(password);
        }

        private bool VerifyUpperCaseLetters(string password)
            => new Regex(@"[A-Z]{1}").IsMatch(password);

        private bool VerifyLowerCaseLetters(string password)
            => new Regex(@"[a-z]{1}").IsMatch(password);

        private bool VerifyIsLetters(string password)
            => new Regex(@"([A-Za-z\s\-]+)").IsMatch(password);

        private bool VerifyIsNumbers(string password)
            => new Regex(@"([0-9])").IsMatch(password);
    }
}
