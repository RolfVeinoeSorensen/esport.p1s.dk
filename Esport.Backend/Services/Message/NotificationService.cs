using Esport.Backend.Data;
using Esport.Backend.Dtos;
using Esport.Backend.Entities;
using Esport.Backend.Extensions;

namespace Esport.Backend.Services.Message
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
        private readonly RazorViewToStringRenderer _razorViewToStringRenderer;
        private readonly DataContext db;

        public NotificationService(
            ILogger<NotificationService> logger,
            RazorViewToStringRenderer razorViewToStringRenderer,
            IEmailSender emailSender,
            DataContext db
        )
        {
            _logger = logger;
            _razorViewToStringRenderer = razorViewToStringRenderer;
            _emailSender = emailSender;
            this.db = db;
        }

        public async Task SendForgotPasswordMail(User user)
        {
            try
            {

                var html = await _razorViewToStringRenderer.RenderViewToStringAsync("Templates/EmailForgotPassword", user);
                var email = new MailDto
                {
                    To = user.Username,
                    Body = html,
                    Subject = "esport.p1s.dk - Reset password"
                };
                await _emailSender.SendEmailAsync(email);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }
        }

        public async Task SendRegisterNewUserConfirmMail(User user)
        {
            try
            {

                var html = await _razorViewToStringRenderer.RenderViewToStringAsync("Templates/EmailConfirmUserRegistration.cshtml", user);
                var email = new MailDto
                {
                    To = user.Username,
                    Body = html,
                    Subject = "esport.p1s.dk - Reset password"
                };
                await _emailSender.SendEmailAsync(email);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.ToString());
            }
        }
    }
}
