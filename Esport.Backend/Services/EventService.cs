using Esport.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Esport.Backend.Dtos;

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

        public Event CreateOrUpdateUserToEvent(EventsUser eventsUser)
        {
            var eu = db.EventsUsers.Any(eu => eu.EventId == eventsUser.EventId && eu.UserId == eventsUser.UserId);
            if (eu == false)
            {
                db.EventsUsers.Add(eventsUser);
                db.SaveChanges();
                return GetEventById(eventsUser.EventId);
            }
            else
            {
                db.EventsUsers.Update(eventsUser);
                db.SaveChanges();
                return GetEventById(eventsUser.EventId);
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

        public Dictionary<string, EventDto> GetAllEvents(int month, int year)
        {
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var datesWithInfo = Extensions.Date.GetMonthYear(month, year);
            var events = db.Events.Include(e => e.EventsUsers).ThenInclude(eu => eu.User)
                .Where(ev =>
                    (ev.StartDateTime.Year == year && ev.StartDateTime.Month == month) ||
                    (ev.EndDateTime.Year == year && ev.EndDateTime.Month == month))
                .ToList();
            Dictionary<string, EventDto> result = [];
            foreach (var dt in datesWithInfo)
            {
                var dto = new EventDto
                {
                    Events = events.Where(ev => (ev.StartDateTime.Date >= dt.Date && ev.EndDateTime.Date <= dt.Date) || (ev.StartDateTime.Date <= dt.Date && ev.EndDateTime.Date >= dt.Date)).ToList(),
                    WeekendWorkday = dt
                };
                result.Add(dt.Date.Date.ToString(), dto);
            }
            return result;
        }

        public Event GetEventById(int id)
        {
            var ev = db.Events
                .Include(e => e.EventsUsers)
                .ThenInclude(eu => eu.User)
                .FirstOrDefault(e => e.Id == id) ?? throw new KeyNotFoundException("Event not found");
            return ev;
        }

        public IEnumerable<EventUserDto> GetUserEventsByUserId(int userId, int month, int year)
        {
            IEnumerable<EventUserDto> result = [];
            return db.EventsUsers
                .Include(eu => eu.User)
                .Include(e => e.Event)
                .Where(eu => eu.UserId == userId && ((eu.Event.StartDateTime.Year >= year && eu.Event.StartDateTime.Month <= month) ||
                    (eu.Event.EndDateTime.Year >= year && eu.Event.EndDateTime.Month >= month)))
                .OrderByDescending(o => o.Event.StartDateTime)
                .Take(10).Select(r => new EventUserDto{ Event = r.Event, EventsUser = r }).ToList();
        }
    }
}