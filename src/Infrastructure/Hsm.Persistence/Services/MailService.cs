using Hsm.Application.Abstractions;
using Hsm.Application.Extensions;
using Hsm.Domain.Models.Options;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Hsm.Persistence.Services
{
    public class MailService(IConfiguration _configuration) : IMailService
    {
        public async Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml, string displayName = "")
        {
            await SendMessageAsync(new[] { to }, subject, body, isBodyHtml, displayName);
        }

        public async Task SendMessageAsync(string[] tos, string subject, string body, bool isBodyHtml = true, string displayName = "")
        {
            EmailOptions emailOptions = _configuration.GetOptions<EmailOptions>("EmailOptions");

            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(emailOptions.From, displayName ?? "", System.Text.Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.UseDefaultCredentials = false;
            smtp.Port = emailOptions.Port;
            smtp.EnableSsl = true;
            smtp.Host = emailOptions.Host;
            smtp.Credentials = new NetworkCredential(emailOptions.Username, emailOptions.Password);

            await smtp.SendMailAsync(mail);
        }

        public async Task SendMessageWithImageAsync(string[] tos, string subject, string body, bool isBodyHtml, string imagePath, string displayName = "")
        {
            EmailOptions emailOptions = _configuration.GetOptions<EmailOptions>("EmailOptions");

            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(emailOptions.From, displayName ?? "", System.Text.Encoding.UTF8);
            mail.Attachments.Add(new Attachment(imagePath));

            SmtpClient smtp = new();
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(emailOptions.Username, emailOptions.Password);
            smtp.Port = emailOptions.Port;
            smtp.EnableSsl = true;
            smtp.Host = emailOptions.Host;
            await smtp.SendMailAsync(mail);
        }
    }
}
