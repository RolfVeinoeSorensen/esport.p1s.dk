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

        public async Task<Event> CreateOrUpdateEvent(EventSubmitRequest ev, AuthUser currentUser)
        {
            var existing = await db.Events.FirstOrDefaultAsync(x => x.Id.Equals(ev.Id));
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
                await db.Events.AddAsync(newEvent);
                await db.SaveChangesAsync();
                return newEvent;
            }
            else
            {
                db.Update(existing);
                await db.SaveChangesAsync();
                return existing;
            }
        }

        public async Task<SubmitResponse> AddTeamToEvent(int eventId, int teamId)
        {
            var ev = db.Events.Include(t => t.Teams).Where(e => e.Id.Equals(eventId)).First();
            var exist = ev.Teams.Where(x => x.Id.Equals(teamId)).FirstOrDefault();
            var response = new SubmitResponse
            {
                Ok = true,
                Message = "Team blev tilføjet til event"
            };
            if (exist == null)
            {
                var team = db.Teams.Find(teamId);
                ev.Teams.Add(team);
                db.UpdateRange(ev.Teams);
                await db.SaveChangesAsync();
            }
            ev.Teams.ToList().ForEach(t =>
            {
                if (t.Id.Equals(teamId))
                {
                    AddMissingUsersFromTeamToEvent(eventId, teamId);
                }
            });
            return response;
        }
        private void AddMissingUsersFromTeamToEvent(int eventId, int teamId)
        {
            List<int> eventsUsers = db.Teams
                .Include(t => t.Members)
                .Where(t => t.Id.Equals(teamId))
                .SelectMany(t => t.Members)
                .Select(m => m.Id)
                .ToList();
            var existing = db.EventsUsers.Where(us => us.EventId.Equals(eventId)).ToList();
            eventsUsers.ForEach(u =>
            {
                var exist = existing.FirstOrDefault(us => us.UserId.Equals(u));
                if (exist == null)
                {
                    EventsUser userEv = new()
                    {
                        EventId = eventId,
                        UserId = u,
                        Invited = DateTime.Now
                    };
                    db.Add(userEv);
                    db.SaveChanges();
                }
            });
        }

        public async Task<Event> CreateOrUpdateUserToEvent(EventsUser eventsUser)
        {
            var eu = db.EventsUsers.Any(eu => eu.EventId == eventsUser.EventId && eu.UserId == eventsUser.UserId);
            if (eu == false)
            {
                await db.EventsUsers.AddAsync(eventsUser);
                await db.SaveChangesAsync();
                return await GetEventById(eventsUser.EventId);
            }
            else
            {
                db.EventsUsers.Update(eventsUser);
                await db.SaveChangesAsync();
                return await GetEventById(eventsUser.EventId);
            }
        }

        public async Task DeleteEvent(int id)
        {
            var ev = await db.Events.FindAsync(id) ?? throw new KeyNotFoundException("Event not found");
            db.Events.Remove(ev);
            await db.SaveChangesAsync();
        }

        public async Task<Event> DeleteUserFromEvent(int eventId, int userId)
        {
            var eu = db.EventsUsers.FirstOrDefault(eu => eu.EventId == eventId && eu.UserId == userId) ?? throw new KeyNotFoundException("User not found in event");
            db.EventsUsers.Remove(eu);
            db.SaveChanges();
            return await GetEventById(eventId);
        }

        public async Task<Dictionary<string, EventDto>> GetAllEvents(int month, int year)
        {
            var firstDayOfMonth = new DateTime(year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var datesWithInfo = Extensions.Date.GetMonthYear(month, year);
            var events = await db.Events.Include(e => e.EventsUsers).ThenInclude(eu => eu.User)
                .Where(ev =>
                    (ev.StartDateTime.Year == year && ev.StartDateTime.Month == month) ||
                    (ev.EndDateTime.Year == year && ev.EndDateTime.Month == month))
                .ToListAsync();
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

        public async Task<Event> GetEventById(int id)
        {
            var ev = await db.Events
                .Include(e => e.Teams)
                .Include(e => e.EventsUsers)
                .ThenInclude(eu => eu.User)
                .AsSplitQuery()
                .FirstOrDefaultAsync(e => e.Id == id) ?? throw new KeyNotFoundException("Event not found");
            return ev;
        }

        public async Task<IEnumerable<EventUserDto>> GetUserEventsByUserId(int userId, int month, int year)
        {
            IEnumerable<EventUserDto> result = [];
            List<EventUserDto> evu = await db.EventsUsers
               .Include(e => e.Event)
               .ThenInclude(e => e.EventsUsers)
               .Where(eu => eu.UserId == userId && ((eu.Event.StartDateTime.Year >= year && eu.Event.StartDateTime.Month <= month) ||
                   (eu.Event.EndDateTime.Year >= year && eu.Event.EndDateTime.Month >= month)))
               .OrderByDescending(o => o.Event.StartDateTime)
               .Take(10).Select(r => new EventUserDto { Event = r.Event, EventsUser = r }).ToListAsync();
            evu.ForEach(e => e.Participants = GetParticipantsCount(e.EventsUser));
            return evu;
        }

        public async Task<IEnumerable<EventUserDto>> GetUpcomingUserEventsByUserId(int userId)
        {
            var dt = DateTime.Now;
            IEnumerable<EventUserDto> result = [];
            List<EventUserDto> evu = await db.EventsUsers
                .Include(e => e.Event)
                .ThenInclude(e => e.EventsUsers)
                .Where(eu => eu.UserId == userId && eu.Event.StartDateTime >= dt)
                .OrderBy(o => o.Event.StartDateTime)
                .Take(10).Select(r => new EventUserDto { Event = r.Event, EventsUser = r }).ToListAsync();
            evu.ForEach(e => e.Participants = GetParticipantsCount(e.EventsUser));
            return evu;
        }

        private static EventParticipants GetParticipantsCount(EventsUser ev)
        {
            var res = new EventParticipants();

            ev.Event.EventsUsers.ToList().ForEach(eu =>
            {
                if (eu.User.CanBringLaptop == true) res.Laptops++;
                if (eu.User.CanBringStationaryPc == true) res.Desktops++;
                if (eu.User.CanBringPlaystation == true) res.Playstations++;

                if (eu.Accepted != null) res.Accepted++;
                if (eu.Declined != null) res.Declined++;
                res.Invited++;
            });

            return res;
        }

        public async Task<SubmitResponse> SaveEventAttendance(AttendEventRequest request, int userId)
        {
            var eventsUser = await db.EventsUsers.FirstOrDefaultAsync(eu => eu.EventId == request.EventId && eu.UserId == userId);
            if (eventsUser == null)
            {
                return new SubmitResponse
                {
                    Ok = false,
                    Message = "Kunne ikke finde deltager til event"
                };
            }
            else
            {
                eventsUser.Accepted = request.Attend == true ? DateTime.Now : null;
                eventsUser.Declined = request.Attend == false ? DateTime.Now : null;
                db.EventsUsers.Update(eventsUser);
                await db.SaveChangesAsync();
                return new SubmitResponse
                {
                    Ok = true,
                    Message = $"Din deltagelse er gemt. Du deltager {(request.Attend == false ? "IKKE " : "")}i eventet."
                };
            }
        }
    }
}