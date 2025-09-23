using Esport.Backend.Entities;

namespace Esport.Backend.Services
{
    public interface IEventService
    {
        IEnumerable<Event> GetAllEvents();
        Event GetEventById(int id);
        Event CreateOrUpdateEvent(Event ev);
        void DeleteEvent(int id);

        Event CreateOrUpdateUserToEvent(int eventId, int userId, DateTime? Invited, DateTime? Accepted, DateTime? Declined);
        Event DeleteUserFromEvent(int eventId, int userId);
    }
}
