using System.ComponentModel.DataAnnotations;

namespace Esport.Backend.Models
{
    public class ContactRequest
    {
        [Required]
        public required string ContactFrom { get; set; }
        [Required]
        public required string ContactName { get; set; }
        [Required]
        public required string Subject { get; set; }
        [Required]
        public required string Body { get; set; }
        [Required]
        public required string CaptchaId { get; set; }
        [Required]
        public required string CaptchaCode { get; set; }
        public string? ContactMobile { get; set; }
    }
}
