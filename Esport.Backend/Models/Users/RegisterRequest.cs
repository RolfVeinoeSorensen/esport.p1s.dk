using System.ComponentModel.DataAnnotations;

namespace Esport.Backend.Models.Users
{
    public class RegisterRequest
    {
        [Required]
        public required string Username { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        public required string PasswordRepeat { get; set; }

        [Required]
        public required string Firstname { get; set; }

        [Required]
        public required string Lastname { get; set; }

        [Required]
        public required string CaptchaId { get; set; }

        [Required]
        public required string CaptchaCode { get; set; }

    }
}
