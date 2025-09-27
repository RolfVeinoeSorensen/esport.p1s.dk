using Esport.Backend.Dtos;
using Esport.Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NSwag.Generation.Processors;

namespace Esport.Backend.Controllers
{
    [Route("captcha")]
    public class CaptchaController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public CaptchaController(IMemoryCache cache)
        {
            _cache = cache;
        }

        // GET /captcha/generate
        [HttpGet("generate")]
        public ActionResult GenerateCaptcha()
        {
            // Generate random CAPTCHA code
            var captchaCode = CaptchaService.GenerateCaptchaCode(6);

            // Create a new CaptchaId
            var CaptchaId = Guid.NewGuid().ToString();

            // Store the code in memory for 10 mins (adjust as needed)
            _cache.Set(CaptchaId, captchaCode, TimeSpan.FromMinutes(10));

            // Generate the image
            var captchaImageBytes = CaptchaService.GenerateCaptchaImage(captchaCode);

            // Convert to Base64
            var base64Image = Convert.ToBase64String(captchaImageBytes);

            // Return JSON: { CaptchaId, CaptchaImage } 
            var dto = new CaptchaDto
            {
                CaptchaId = CaptchaId,
                CaptchaImage = $"data:image/png;base64,{base64Image}"
            };
            return Ok(dto);
        }

        // GET /captcha/refresh?CaptchaId=<your-guid-here>
        [HttpGet("refresh")]
        public ActionResult RefreshCaptcha(string CaptchaId)
        {
            if (string.IsNullOrEmpty(CaptchaId))
            {
                return BadRequest("CaptchaId is required.");
            }

            // Remove existing captcha code from cache
            _cache.Remove(CaptchaId);

            // Generate a new code
            var newCaptchaCode = CaptchaService.GenerateCaptchaCode(6);

            // Store it in memory
            _cache.Set(CaptchaId, newCaptchaCode, TimeSpan.FromMinutes(10));

            // Generate the new image
            var captchaImageBytes = CaptchaService.GenerateCaptchaImage(newCaptchaCode);

            // Convert to Base64
            var base64Image = Convert.ToBase64String(captchaImageBytes);

            // Return JSON
            var dto = new CaptchaDto
            {
                CaptchaId = CaptchaId,
                CaptchaImage = $"data:image/png;base64,{base64Image}"
            };
            return Ok(dto);
        }
    }
}