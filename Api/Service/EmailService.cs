

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace shopbackend.Api.Service
{
    public class EmailService:IEmailService
    {
        private readonly EmailSetting emailSetting;
        public EmailService(IOptions<EmailSetting> options) { 
        
            this.emailSetting = options.Value;
        }
        public async Task SendEmailAsync(MailSending mailSending)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(emailSetting.Email);
            email.To.Add(MailboxAddress.Parse(mailSending.ToEmail));
            email.Subject = mailSending.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mailSending.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(emailSetting.Host, emailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(emailSetting.Email,emailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);

            throw new NotImplementedException();
        }
    }
}
