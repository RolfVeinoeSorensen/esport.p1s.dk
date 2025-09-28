using Esport.Backend.Models;

namespace Esport.Backend.Services
{
    public interface IContactService
    {
        Task<SubmitResponse> CreateContact(ContactRequest req);
    }
}
