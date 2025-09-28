using System.ComponentModel.DataAnnotations;

namespace Esport.Backend.Models.Users
{
    public class ChangePasswordRequest
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
