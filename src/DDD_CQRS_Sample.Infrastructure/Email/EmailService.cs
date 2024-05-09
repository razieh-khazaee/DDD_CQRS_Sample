using Microsoft.Extensions.Options;
using Shared.Email;
using System.Net;
using System.Net.Mail;

namespace DDD_CQRS_Sample.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettingOptions emailSetting;

        public EmailService(IOptions<EmailSettingOptions> emailSettingOptions)
        {
            emailSetting = emailSettingOptions.Value;
        }

        public async Task SendEmail(string to, string subject, string body)
        {
            using (var client = new SmtpClient(emailSetting.Host, emailSetting.Port))
            {
                client.Credentials = new NetworkCredential(emailSetting.UserName, emailSetting.Password);
                client.EnableSsl = true;

                var mail = new MailMessage()
                {
                    From = new MailAddress(emailSetting.UserName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(to);

                await client.SendMailAsync(mail);
            }
        }
    }
}
