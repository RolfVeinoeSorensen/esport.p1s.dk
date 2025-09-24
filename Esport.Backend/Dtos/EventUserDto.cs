using Esport.Backend.Entities;

namespace Esport.Backend.Dtos
{
    public class EventUserDto
    {
        public required Event Event {  get; set; }
        public required EventsUser EventsUser { get; set; }
    }
}
