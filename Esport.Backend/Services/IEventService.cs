using Esport.Backend.Entities;

namespace Esport.Backend.Services
{
    public interface IEventService
    {
        IEnumerable<Event> GetAllEvents(DateTime startDateTime, DateTime endDateTime);
        Event GetEventById(int id);
        Event CreateOrUpdateEvent(Event ev);
        void DeleteEvent(int id);

        Event CreateOrUpdateUserToEvent(EventsUser eventsUser);
        Event DeleteUserFromEvent(int eventId, int userId);
    }
}
