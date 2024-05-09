namespace Shared.Email
{
    public interface IEmailService
    {
        Task SendEmail(string to, string subject, string body);
    }
}