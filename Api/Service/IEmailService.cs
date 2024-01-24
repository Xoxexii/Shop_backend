namespace shopbackend.Api.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailSending mailSending);
    }
}
