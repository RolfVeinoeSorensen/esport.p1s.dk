using System.ComponentModel.DataAnnotations;

namespace Esport.Backend.Models
{
    public class UserToTeamRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TeamId { get; set; }
    }
}
