using Esport.Backend.Dtos;
using Esport.Backend.Entities;
using Esport.Backend.Models;

namespace Esport.Backend.Services
{
    public interface IEventService
    {
        Dictionary<string, EventDto> GetAllEvents(int month, int year);
        Event GetEventById(int id);
        Event CreateOrUpdateEvent(EventSubmitRequest ev, AuthUser currentUser);
        void DeleteEvent(int id);
        Event CreateOrUpdateUserToEvent(EventsUser eventsUser);
        Event DeleteUserFromEvent(int eventId, int userId);
        IEnumerable<EventUserDto> GetUserEventsByUserId(int userId, int month, int year);
    }
}
