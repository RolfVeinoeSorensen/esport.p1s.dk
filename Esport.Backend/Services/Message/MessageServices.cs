using HtmlAgilityPack;
using MailKit.Net.Smtp;
using MimeKit;
using Esport.Backend.Dtos;

namespace Esport.Backend.Services.Message
{
    public class MessageServices : ISmsSender, IEmailSender
    {
        public async Task SendEmailAsync(MailDto mail)
        {
            var mailMessage = new MimeMessage();
            mailMessage.To.Add(new MailboxAddress(mail.To, mail.To));
            mailMessage.Bcc.Add(new MailboxAddress("mail@hourplanner.com", "mail@hourplanner.com"));
            mailMessage.From.Add(new MailboxAddress("Hourplanner.com", "mail@hourplanner.com"));
            mailMessage.Subject = mail.Subject;

            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new Multipart("mixed");
            //Create a pure text part
            HtmlDocument mainDoc = new HtmlDocument();
            string htmlString = mail.Body;
            mainDoc.LoadHtml(htmlString);
            string cleanText = mainDoc.DocumentNode.InnerText;

            var alternative = new MultipartAlternative
            {
                new TextPart(MimeKit.Text.TextFormat.Plain) { Text = cleanText },
                new TextPart(MimeKit.Text.TextFormat.Html) { Text = mail.Body }
            };
            multipart.Add(alternative);

            if (mail.Attachments != null)
            {
                foreach (var attachment in mail.Attachments.ToList())
                {
                    multipart.Add(attachment);
                }
            }

            mailMessage.Body = multipart;
            using var client = new SmtpClient();
            var useSSL = MailKit.Security.SecureSocketOptions.StartTls;
            await client.ConnectAsync("mail.hourplanner.com", 587, useSSL);

            // Note: since we don't have an OAuth2 token, disable
            // the XOAUTH2 authentication mechanism.
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            await client.AuthenticateAsync("mail@hourplanner.com", "xqfLBcFv4ishRp");
            await client.SendAsync(mailMessage);
            await client.DisconnectAsync(true);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // TODO: Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
