using System.Collections.Generic;
using System.Threading.Tasks;
using WaterHub.Core.Models;

namespace WaterHub.Core.Abstractions
{
    public interface ISmtpService
    {
        Task<ProcessResult> SendMessagesAsync
            (EmailContact from, IEnumerable<EmailContact> to, string subject, string body, bool isHtml = false);

        Task<ProcessResult> SendMessagesAsync
            (EmailContact from, EmailContact to, string subject, string body, bool isHtml = false);
    }
}