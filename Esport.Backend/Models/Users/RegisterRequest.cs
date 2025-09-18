using System.ComponentModel.DataAnnotations;

namespace Esport.Backend.Models.Users
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
