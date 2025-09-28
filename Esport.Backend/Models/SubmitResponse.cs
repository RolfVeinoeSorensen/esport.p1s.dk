using Esport.Backend.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Esport.Backend.Models
{
    public class SubmitResponse
    {
        [Required]
        public required bool Ok { get; set; }
        public string? Message { get; set; }
        public CaptchaDto? Captcha { get; set; }
    }
}
