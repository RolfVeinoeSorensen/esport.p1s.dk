using MimeKit;

namespace Esport.Backend.Dtos
{
    public class MailDto
    {
        public string To {  get; set; }
        public string Body {  get; set; }
        public string Subject {  get; set; }
        public List<MimeEntity> Attachments {  get; set; }
    }
}
