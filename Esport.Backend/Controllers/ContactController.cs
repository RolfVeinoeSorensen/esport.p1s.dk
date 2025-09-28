using Esport.Backend.Models;
using Esport.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Esport.Backend.Controllers
{
    public class ContactController(IContactService contactService) : ControllerBase
    {
        private readonly IContactService cs = contactService;

        [HttpPost("[action]")]
        public async Task<SubmitResponse> CreateContact([FromBody]ContactRequest req)
        {
            return await cs.CreateContact(req);
        }
    }
}
