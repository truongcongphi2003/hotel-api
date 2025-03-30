using hotel_booking_core.Interfaces;
using hotel_booking_models.Mail;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_core.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<MailService> _logger;
        public MailService(MailSettings mailSettings, ILogger<MailService> logger)
        {
            _mailSettings = mailSettings;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.FromMail));
            email.To.Add(new MailboxAddress("", mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                foreach (var file in mailRequest.Attachments)
                {
                    byte[] fileBytes;
                    await using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    builder.Attachments.Add((file.FileName + Guid.NewGuid().ToString()));
                }
            }

            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            try
            {
                using (var smtp = new SmtpClient())
                {
                    await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_mailSettings.FromMail, _mailSettings.Password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Source, ex.InnerException, ex.Message, ex.ToString());
                return false;
            }

        }
    }
}
