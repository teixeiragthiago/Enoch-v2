using Enoch.CrossCutting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace CrossCutting.Email
{
    public class Email
    {
        public void Send(string from, string mailPassWord, string to, string subject, string file)
        {
            file = file.Replace("[YEAR]", $"{DateTime.Now.Year}");
            var parameters = new Parameters();

            var smtp = new SmtpClient
            {
                Host = parameters.Data.Mail.Host,
                Port = parameters.Data.Mail.Port,
                EnableSsl = parameters.Data.Mail.SSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, Encryption.Decrypt(mailPassWord))
            };

            using (var message = new MailMessage(from, to))
            {
                message.Subject = subject;
                message.Body = file;
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
        }

        public void Send(string from, string mailPassWord, string to, string subject, string file, string cc = null, IEnumerable<AttachmentFile> attachments = null)
        {
            file = file.Replace("[YEAR]", $"{DateTime.Now.Year}");
            var parameters = new Parameters();

            var smtp = new SmtpClient
            {
                Host = parameters.Data.Mail.Host,
                Port = parameters.Data.Mail.Port,
                EnableSsl = parameters.Data.Mail.SSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(from, Encryption.Decrypt(mailPassWord))
            };

            using (var message = new MailMessage(from, to))
            {
                message.Subject = subject;
                message.Body = file;
                message.IsBodyHtml = true;

                if (!string.IsNullOrEmpty(cc))
                {
                    foreach (var item in cc.Split(";"))
                        message.CC.Add(item);
                }

                foreach (var attachment in attachments.ToList())
                    message.Attachments.Add(new Attachment(new MemoryStream(attachment.File), attachment.Name));

                smtp.Send(message);
            }
        }
    }

    public class AttachmentFile
    {
        public string Name { get; set; }
        public byte[] File { get; set; }
    }
}
