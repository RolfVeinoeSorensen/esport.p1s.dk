using Esport.Backend.Dtos;
using Esport.Backend.Entities;

namespace Esport.Backend.Services
{
    public interface IEventService
    {
        Dictionary<string, EventDto> GetAllEvents(int month, int year);
        Event GetEventById(int id);
        Event CreateOrUpdateEvent(Event ev);
        void DeleteEvent(int id);

        Event CreateOrUpdateUserToEvent(EventsUser eventsUser);
        Event DeleteUserFromEvent(int eventId, int userId);
    }
}
