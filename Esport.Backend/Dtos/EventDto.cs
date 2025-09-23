using Esport.Backend.Entities;

namespace Esport.Backend.Dtos
{
    public class EventDto
    {
        public required IEnumerable<Event> Events {  get; set; }
        public required HolidayWeekendWorkdayDto WeekendWorkday { get; set; }
    }
}
