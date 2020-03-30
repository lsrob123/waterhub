using System.Collections.Generic;
using System.Threading.Tasks;
using WaterHub.Core.Models;

namespace WaterHub.Core.Abstractions
{
    public interface ISmtpService
    {
        Task<ProcessResult> SendMessagesAsync
            (EmailAccount from, IEnumerable<EmailAccount> to, string subject, string body, bool isHtml = false);

        Task<ProcessResult> SendMessagesAsync
            (EmailAccount from, EmailAccount to, string subject, string body, bool isHtml = false);
    }
}