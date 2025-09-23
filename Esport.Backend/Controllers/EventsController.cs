using Esport.Backend.Authorization;
using Esport.Backend.Entities;
using Esport.Backend.Enums;
using Esport.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Esport.Backend.Controllers
{
    public class EventsController(IEventService eventService) : ControllerBase
    {
        private readonly IEventService eventService = eventService;

        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Event>> GetDates(int month, int year)
        {
            var dates = Extensions.Date.GetMonthYear(month, year);
            return Ok(dates);
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Event>> GetAllEvents(DateTime startDateTime, DateTime endDateTime)
        {
            var events = eventService.GetAllEvents(startDateTime, endDateTime);
            return Ok(events);
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpGet("[action]/{id:int}")]
        public ActionResult<Event> GetById(int id)
        {
            var eventResult = eventService.GetEventById(id);
            return Ok(eventResult);
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpPost("[action]")]
        public ActionResult<Event> CreateOrUpdateEvent([FromBody]Event ev)
        {
            if (HttpContext.Items["User"] is not User currentUser || (currentUser.Role != UserRole.Admin))
                return Unauthorized(new { message = "Unauthorized" });

            var eventResult = eventService.CreateOrUpdateEvent(ev);
            return Ok(eventResult);
        }
        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpDelete("[action]")]
        public ActionResult<Event> DeleteEvent(int id)
        {
            if (HttpContext.Items["User"] as User is not User currentUser || (currentUser.Role != UserRole.Admin))
                return Unauthorized(new { message = "Unauthorized" });

            eventService.DeleteEvent(id);
            return Ok();
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpPost("[action]")]
        public ActionResult<Event> CreateOrUpdateUserToEvent([FromBody]EventsUser eventsUser)
        {
            if (HttpContext.Items["User"] is not User currentUser || currentUser.Role != UserRole.Admin || currentUser.Id != eventsUser.UserId)
                return Unauthorized(new { message = "Unauthorized" });

            var eventResult = eventService.CreateOrUpdateUserToEvent(eventsUser);
            return Ok(eventResult);
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpDelete("[action]")]
        public ActionResult<Event> DeleteUserFromEvent(int eventId, int userId)
        {
            if (HttpContext.Items["User"] is not User currentUser || currentUser.Role != UserRole.Admin || currentUser.Id != userId)
                return Unauthorized(new { message = "Unauthorized" });

            var eventResult = eventService.DeleteUserFromEvent(eventId, userId);
            return Ok(eventResult);
        }
    }
}
