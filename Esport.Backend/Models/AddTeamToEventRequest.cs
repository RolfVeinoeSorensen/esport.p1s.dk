using System.ComponentModel.DataAnnotations;

namespace Esport.Backend.Models
{
    public class AddTeamToEventRequest
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        public int TeamId { get; set; }
    }
}
