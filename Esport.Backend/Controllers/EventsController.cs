using Esport.Backend.Authorization;
using Esport.Backend.Dtos;
using Esport.Backend.Entities;
using Esport.Backend.Enums;
using Esport.Backend.Models;
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
        public async Task<ActionResult<Dictionary<string, EventDto>>> GetAllEvents(int month, int year)
        {
            var events = await eventService.GetAllEvents(month, year);
            return Ok(events);
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpGet("[action]/{id:int}")]
        public async Task<ActionResult<Event>> GetById(int id)
        {
            var eventResult = await eventService.GetEventById(id);
            return Ok(eventResult);
        }

        [Authorize(UserRole.Admin)]
        [HttpPost("[action]")]
        public async Task<ActionResult<Event>> CreateOrUpdateEvent([FromBody] EventSubmitRequest ev)
        {
            if (HttpContext.Items["User"] is not AuthUser currentUser)
                return Unauthorized(new { message = "Unauthorized" });

            var eventResult = await eventService.CreateOrUpdateEvent(ev, currentUser);
            return Ok(eventResult);
        }
        [Authorize(UserRole.Admin)]
        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            await eventService.DeleteEvent(id);
            return Ok();
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpPost("[action]")]
        public async Task<ActionResult<Event>> CreateOrUpdateUserToEvent([FromBody] EventsUser eventsUser)
        {
            if (HttpContext.Items["User"] is not AuthUser currentUser || currentUser.Roles.Any(x => x.Role.Equals(UserRole.Admin)) == true || currentUser.Id != eventsUser.UserId)
                return Unauthorized(new { message = "Unauthorized" });

            var eventResult = await eventService.CreateOrUpdateUserToEvent(eventsUser);
            return Ok(eventResult);
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpDelete("[action]")]
        public async Task<ActionResult<Event>> DeleteUserFromEvent(int eventId, int userId)
        {
            if (HttpContext.Items["User"] is not AuthUser currentUser || currentUser.Roles.Any(x => x.Role.Equals(UserRole.Admin)) == true || currentUser.Id != userId)
                return Unauthorized(new { message = "Unauthorized" });

            var eventResult = await eventService.DeleteUserFromEvent(eventId, userId);
            return Ok(eventResult);
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<EventUserDto>>> GetUserEventsByUserId(int month, int year)
        {
            if (HttpContext.Items["User"] is not AuthUser currentUser)
                return Unauthorized(new { message = "Unauthorized" });
            var events = await eventService.GetUserEventsByUserId(currentUser.Id, month, year);
            return Ok(events);
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<EventUserDto>>> GetUpcomingUserEventsByUserId()
        {
            if (HttpContext.Items["User"] is not AuthUser currentUser)
                return Unauthorized(new { message = "Unauthorized" });
            var events = await eventService.GetUpcomingUserEventsByUserId(currentUser.Id);
            return Ok(events);
        }

        [Authorize(UserRole.Admin)]
        [HttpPost("[action]")]
        public async Task<ActionResult<Event>> AddTeamToEvent([FromBody] AddTeamToEventRequest req)
        {
            {
                var eventResult = await eventService.AddTeamToEvent(req.EventId, req.TeamId);
                return Ok(eventResult);
            }
        }

        [Authorize([UserRole.Admin, UserRole.MemberAdult, UserRole.MemberKid])]
        [HttpPost("[action]")]
        public async Task<ActionResult<SubmitResponse>> SaveEventAttendance([FromBody] AttendEventRequest request)
        {
            if (HttpContext.Items["User"] is not AuthUser currentUser)
                return Unauthorized(new { message = "Unauthorized" });

            var response = await eventService.SaveEventAttendance(request, currentUser.Id);
            return Ok(response);
        }
    }
}
