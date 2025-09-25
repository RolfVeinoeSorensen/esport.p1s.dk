using Esport.Backend.Entities;

namespace Esport.Backend.Services.Message
{
    public interface INotificationService
    {
        Task SendForgotPasswordMail(AuthUser user);
        Task SendRegisterNewUserConfirmMail(AuthUser user);
    }
}
