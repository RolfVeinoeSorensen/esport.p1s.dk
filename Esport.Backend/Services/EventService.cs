using Esport.Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Esport.Backend.Dtos;
using Esport.Backend.Models;

namespace Esport.Backend.Services
{
    public class EventService(
        DataContext context) : IEventService
    {

        private readonly DataContext db = context;

        public Event CreateOrUpdateEvent(EventSubmitRequest ev, AuthUser currentUser)
        {
            var existing = db.Events.FirstOrDefault(x => x.Id.Equals(ev.Id));
            if (existing == null)
            {
                Event newEvent = new()
                {
                    CreatedBy = currentUser.Id,
                    CreatedDateTime = DateTime.Now,
                    Name = ev.Name,
                    Description = ev.Description,
                    StartDateTime = ev.StartDateTime,
                    EndDateTime = ev.EndDateTime,
                };
                db.Events.Add(newEvent);
                db.SaveChanges();
                return newEvent;
            }
            else
            {
                db.Update(existing);
                db.SaveChanges();
                return existing;
            }
        }

        private void AddTeamsToEvent(int eventId, ICollection<int> EventsUsers)
        {

        }
        private void AddUsersToEvent(int eventId, ICollection<int> EventsTeams)
        {

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
                .Take(10).Select(r => new EventUserDto { Event = r.Event, EventsUser = r }).ToList();
        }
    }
}