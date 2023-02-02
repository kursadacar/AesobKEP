using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aesob.Web.Library.Service;
using System.Net.WebSockets;
using Aesob.Web.Library;

namespace Aesob.Web.Core.Public
{
    public class EMailService : IAesobService
    {
        private static bool _isSendingEmail;

        private static Queue<MailData> _pendingEMails = new Queue<MailData>();

        private string _userName;
        private string _password;

        void IAesobService.Start()
        {
            var thisAsInterface = (IAesobService)this;

            _userName = thisAsInterface.GetConfig("Username");
            _password = thisAsInterface.GetConfig("Password");
        }

        void IAesobService.Update(float dt)
        {
        }

        void IAesobService.Stop()
        {
        }

        public void SendMail(MailData mailData)
        {
            if (mailData.TargetAddresses.Length == 0)
            {
                return;
            }

            if (_isSendingEmail)
            {
                _pendingEMails.Enqueue(mailData);
                return;
            }

            _isSendingEmail = true;

            try
            {
                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(_userName, mailData.SenderAlias);

                mailMessage.Subject = mailData.Subject;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = mailData.Content;

                SmtpClient smtpClient = new SmtpClient("srvm11.trwww.com");
                smtpClient.Port = 587;
                //smtpClient.UseDefaultCredentials = true;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential()
                {
                    UserName = _userName,
                    Password = _password
                };

                SendMailToAddresses(smtpClient, mailMessage, mailData.TargetAddresses);
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine(e.Message);
                sb.AppendLine();
                sb.AppendLine(e.InnerException.Message);
                sb.AppendLine();
                sb.AppendLine(e.StackTrace);

                var errMessage = sb.ToString();
                Debug.Log(errMessage);
            }

            _isSendingEmail = false;

            if (_pendingEMails.Count > 0)
            {
                var mail = _pendingEMails.Dequeue();
                SendMail(mail);
            }
        }

        private static async void SendMailToAddresses(SmtpClient client, MailMessage message, string[] addresses)
        {
            for (int i = 0; i < addresses.Length; i++)
            {
                if (!string.IsNullOrEmpty(addresses[i]))
                {
                    message.To.Add(addresses[i].Trim());
                    client.Send(message);
                    message.To.Clear();
                    await Task.Delay(10);
                }
            }
        }

        public struct MailData
        {
            public string SenderAlias { get; set; }
            public string Subject { get; set; }
            public string Content { get; set; }
            public string[] TargetAddresses { get; set; }

            public MailData(string senderAlias, string subject, string content, params string[] targetAddresses)
            {
                SenderAlias = senderAlias;
                Subject = subject;
                Content = content;
                TargetAddresses = targetAddresses;
            }
        }
    }
}
