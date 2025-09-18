using Esport.Backend.Dtos;

namespace Esport.Backend.Services.Message
{
    public interface IEmailSender
    {
        Task SendEmailAsync(MailDto mail);
    }
}
