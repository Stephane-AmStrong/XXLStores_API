using Application.DataTransfertObjects.Email;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Shared.Service
{
    public class EmailService
    {
        private readonly MailSettings _mailSettings;
        public ILogger<EmailService> _logger { get; }

        public EmailService(MailSettings mailSettings)
        {
            _mailSettings = mailSettings;
        }

        public void Send(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        public async Task SendAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }


        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            //emailMessage.From.Add(new MailboxAddress(_emailConfig.Name, _emailConfig.EmailId));
            emailMessage.Sender = new MailboxAddress(_mailSettings.DisplayName, message.From ?? _mailSettings.EmailFrom);
            emailMessage.To.Add(MailboxAddress.Parse(message.To));
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder
            {
                //HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) 
                HtmlBody = message.Content
            };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var smtp = new SmtpClient();
            try
            {
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);

                smtp.Send(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception, or both.
                throw;
            }
            finally
            {
                smtp.Disconnect(true);
                smtp.Dispose();
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                await smtp.AuthenticateAsync(_mailSettings.SmtpUser, _mailSettings.SmtpPass);

                await smtp.SendAsync(mailMessage);
            }
            catch
            {
                //log an error message or throw an exception, or both.
                throw;
            }
            finally
            {
                await smtp.DisconnectAsync(true);
                smtp.Dispose();
            }
        }
    }
}
