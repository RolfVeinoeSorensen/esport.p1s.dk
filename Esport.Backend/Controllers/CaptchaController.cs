using Esport.Backend.Dtos;
using Esport.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Esport.Backend.Controllers
{
    [Route("captcha")]
    public class CaptchaController : ControllerBase
    {
        private readonly ICaptchaService cs;

        public CaptchaController(ICaptchaService captchaService)
        {
            cs = captchaService;
        }

        // GET /captcha/generate
        [HttpGet("generate")]
        public ActionResult<CaptchaDto> GenerateCaptcha()
        {
            var dto = cs.GenerateCaptcha();
            return Ok(dto);
        }

        // GET /captcha/refresh?CaptchaId=<your-guid-here>
        [HttpGet("refresh")]
        public ActionResult<CaptchaDto> RefreshCaptcha(string CaptchaId)
        {
            var dto = cs.RefreshCaptcha(CaptchaId);
            return Ok(dto);
        }
    }
}