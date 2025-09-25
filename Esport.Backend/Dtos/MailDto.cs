using MimeKit;

namespace Esport.Backend.Dtos
{
    public class MailDto
    {
        public required string To {  get; set; }
        public required string Body {  get; set; }
        public required string Subject {  get; set; }
        public List<MimeEntity> Attachments {  get; set; }
    }
}
