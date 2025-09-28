using Esport.Backend.Dtos;
using Esport.Backend.Entities;
using Esport.Backend.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Esport.Backend.Services
{
    public class ContactService(
        DataContext context, IMemoryCache memoryCache) : IContactService
    {
        private readonly DataContext db = context;
        private readonly IMemoryCache mc = memoryCache;

        public async Task<ContactResponse> CreateContact(ContactRequest req)
        {
            ContactResponse response = new ContactResponse
            {
                Ok = true,
            };
            mc.TryGetValue(req.CaptchaId, out string? storedCaptchaCode);
            if (storedCaptchaCode == null || storedCaptchaCode != req.CaptchaCode) {
                response.Ok = false;
                response.Message = "CAPTCHA var ikke udfyldt korrekt";
                response.Captcha = GenerateCaptcha();
            } else
            {
                var contact = new Contact
                {
                    Body = req.Body,
                    ContactFrom = req.ContactFrom,
                    ContactMobile = req.ContactMobile,
                    ContactName = req.ContactName,
                    Subject = req.Subject,
                };
                await db.AddAsync(contact);
                await db.SaveChangesAsync();
                response.Message = "Tak for din henvendelse. Vi vender tilbage til dig snarest muligt";
            }
            mc.Remove(req.CaptchaId);
            return response;
        }
        private CaptchaDto GenerateCaptcha()
        {
            // Generate random CAPTCHA code
            var captchaCode = CaptchaService.GenerateCaptchaCode(6);

            // Create a new CaptchaId
            var CaptchaId = Guid.NewGuid().ToString();

            // Store the code in memory for 10 mins (adjust as needed)
            mc.Set(CaptchaId, captchaCode, TimeSpan.FromMinutes(10));

            // Generate the image
            var captchaImageBytes = CaptchaService.GenerateCaptchaImage(captchaCode);

            // Convert to Base64
            var base64Image = Convert.ToBase64String(captchaImageBytes);

            return new CaptchaDto
            {
                CaptchaId = CaptchaId,
                CaptchaImage = $"data:image/png;base64,{base64Image}"
            };
        }
    }
}
