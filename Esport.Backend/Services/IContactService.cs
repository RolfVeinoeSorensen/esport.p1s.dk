using Esport.Backend.Models;

namespace Esport.Backend.Services
{
    public interface IContactService
    {
        Task<ContactResponse> CreateContact(ContactRequest req);
    }
}
