using System.ComponentModel.DataAnnotations;

namespace Esport.Backend.Models
{
    public class EventSubmitRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
    }
}
