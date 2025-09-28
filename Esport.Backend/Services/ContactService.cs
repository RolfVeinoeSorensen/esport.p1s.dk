using Esport.Backend.Entities;
using Esport.Backend.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Esport.Backend.Services
{
    public class ContactService(
        DataContext context, IMemoryCache memoryCache, ICaptchaService captchaService) : IContactService
    {
        private readonly DataContext db = context;
        private readonly IMemoryCache mc = memoryCache;
        private readonly ICaptchaService cs = captchaService;

        public async Task<SubmitResponse> CreateContact(ContactRequest req)
        {
            var response = new SubmitResponse
            {
                Ok = true,
            };
            mc.TryGetValue(req.CaptchaId, out string? storedCaptchaCode);
            if (storedCaptchaCode == null || storedCaptchaCode != req.CaptchaCode) {
                response.Ok = false;
                response.Message = "CAPTCHA var ikke udfyldt korrekt";
                response.Captcha = cs.RefreshCaptcha(req.CaptchaId);
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
    }
}
