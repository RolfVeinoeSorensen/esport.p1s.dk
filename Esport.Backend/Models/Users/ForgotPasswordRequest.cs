using System.ComponentModel.DataAnnotations;

namespace Esport.Backend.Models.Users
{
    public class ForgotPasswordRequest
    {
        [Required]
        public required string Email { get; set; }

        [Required]
        public required string CaptchaId { get; set; }

        [Required]
        public required string CaptchaCode { get; set; }
    }
}
