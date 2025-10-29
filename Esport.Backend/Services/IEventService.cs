using Esport.Backend.Dtos;
using Esport.Backend.Entities;
using Esport.Backend.Models;

namespace Esport.Backend.Services
{
    public interface IEventService
    {
        Task<Dictionary<string, EventDto>> GetAllEvents(int month, int year);
        Task<Event> GetEventById(int id);
        Task<Event> CreateOrUpdateEvent(EventSubmitRequest ev, AuthUser currentUser);
        Task DeleteEvent(int id);
        Task<Event> CreateOrUpdateUserToEvent(EventsUser eventsUser);
        Task<Event> DeleteUserFromEvent(int eventId, int userId);
        Task<IEnumerable<EventUserDto>> GetUserEventsByUserId(int userId, int month, int year);
        Task<IEnumerable<EventUserDto>> GetUpcomingUserEventsByUserId(int userId);
        Task<SubmitResponse> AddTeamToEvent(int eventId, int teamId);
    }
}
