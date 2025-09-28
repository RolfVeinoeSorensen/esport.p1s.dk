using Esport.Backend.Dtos;

namespace Esport.Backend.Services
{
    public interface ICaptchaService
    {
        CaptchaDto GenerateCaptcha();
        CaptchaDto RefreshCaptcha(string CaptchaId);
    }
}
