using System.ComponentModel.DataAnnotations;

namespace Esport.Backend.Models
{
    public class AttendEventRequest
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        public bool Attend { get; set; }
    }
}