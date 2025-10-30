using Esport.Backend.Entities;

namespace Esport.Backend.Dtos
{
    public class EventUserDto
    {
        public required Event Event { get; set; }
        public required EventsUser EventsUser { get; set; }
        public EventParticipants? Participants { get; set; }

    }

    public class EventParticipants
    {
        public int Accepted { get; set; } = 0;
        public int Declined { get; set; } = 0;
        public int Invited { get; set; } = 0;
        public int Laptops { get; set; } = 0;
        public int Desktops { get; set; } = 0;
        public int Playstations { get; set; } = 0;
    }
}
