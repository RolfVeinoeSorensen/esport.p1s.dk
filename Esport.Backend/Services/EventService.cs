using Esport.Backend.Entities;
using Esport.Backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Esport.Backend.Services
{
    public class EventService(
        DataContext context) : IEventService
    {

        private readonly DataContext db = context;

        public Event CreateOrUpdateEvent(Event ev)
        {
            if (ev.Id == 0)
            {
                db.Events.Add(ev);
                db.SaveChanges();
                return ev;
            }
            else
            {
                db.Update(ev);
                db.SaveChanges();
                return ev;
            }
        }

        public Event CreateOrUpdateUserToEvent(int eventId, int userId, DateTime? Invited, DateTime? Accepted, DateTime? Declined)
        {
            var eu = db.EventsUsers.FirstOrDefault(eu => eu.EventId == eventId && eu.UserId == userId);
            if (eu == null)
            {
                eu = new EventsUser
                {
                    EventId = eventId,
                    UserId = userId,
                    Invited = Invited,
                    Accepted = Accepted,
                    Declined = Declined
                };
                db.EventsUsers.Add(eu);
                db.SaveChanges();
                return GetEventById(eventId);
            }
            else
            {
                eu.Invited = Invited;
                eu.Accepted = Accepted;
                eu.Declined = Declined;
                db.EventsUsers.Update(eu);
                db.SaveChanges();
                return GetEventById(eventId);
            }
        }

        public void DeleteEvent(int id)
        {
            var ev = db.Events.Find(id) ?? throw new KeyNotFoundException("Event not found");
            db.Events.Remove(ev);
            db.SaveChanges();
        }

        public Event DeleteUserFromEvent(int eventId, int userId)
        {
            var eu = db.EventsUsers.FirstOrDefault(eu => eu.EventId == eventId && eu.UserId == userId) ?? throw new KeyNotFoundException("User not found in event");
            db.EventsUsers.Remove(eu);
            db.SaveChanges();
            return GetEventById(eventId);
        }

        public IEnumerable<Event> GetAllEvents(DateTime startDateTime, DateTime endDateTime)
        {
            return [.. db.Events.Include(e => e.EventsUsers).ThenInclude(eu => eu.User).Where(ev=> ev.StartDateTime >= startDateTime && ev.EndDateTime <= endDateTime)];
        }

        public Event GetEventById(int id)
        {
            var ev = db.Events
                .Include(e => e.EventsUsers)
                .ThenInclude(eu => eu.User)
                .FirstOrDefault(e => e.Id == id) ?? throw new KeyNotFoundException("Event not found");
            return ev;
        }
    }
}