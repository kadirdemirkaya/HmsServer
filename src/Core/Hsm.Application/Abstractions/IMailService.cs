using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsm.Application.Abstractions
{
    public interface IMailService
    {
        Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml = true, string displayName = "");
        Task SendMessageAsync(string[] tos, string subject, string body, bool isBodyHtml = true, string displayName = "");
        Task SendMessageWithImageAsync(string[] tos, string subject, string body, bool isBodyHtml, string imagePath, string displayName = "");
    }
}
