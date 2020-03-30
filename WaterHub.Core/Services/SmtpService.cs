using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaterHub.Core;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

public class SmtpService : ISmtpService
{
    protected readonly ILogger<SmtpService> Logger;
    protected readonly SmtpSettings Settings;

    public SmtpService(IHasSmtpSettings hasSmtpSettings, ILogger<SmtpService> logger)
    {
        Settings = hasSmtpSettings.SmtpSettings;
        Logger = logger;
    }

    public virtual async Task<ProcessResult> SendMessagesAsync
        (EmailAccount from, IEnumerable<EmailAccount> to, string subject, string body, bool isHtml = false)
    {

        var messages = to
           .Select(x => CreateMimeMessage(from, to, subject, body, isHtml))
           .ToList();

        var exceptions = new List<Exception>();

        using var client = new SmtpClient();
        try
        {
            client.Connect(Settings.Host, Settings.Port, SecureSocketOptions.SslOnConnect);

            client.Authenticate(Settings.SmtpUsername, Settings.SmtpPassword);

            foreach (var message in messages)
            {
                try
                {
                    await client.SendAsync(message);
                }
                catch (Exception individualSendException)
                {
                    exceptions.Add(individualSendException);
                }
            }

            client.Disconnect(true);

            if (!exceptions.Any())
                return new ProcessResult().AsOk();

            return Logger.LogAndReturnProcessResult(new AggregateException(exceptions), "Failed to send to individual recipients");
        }
        catch (Exception exception)
        {
            return Logger.LogAndReturnProcessResult(exception, "Failed to send message");
        }
    }

    private MimeMessage CreateMimeMessage(EmailAccount from, IEnumerable<EmailAccount> to, string subject,
        string body, bool isHtml = false)
    {
        var originalSender = from.HasAccountName ? $"\"{from.Name}\" {from.Address}" : from.Address;

        var mimeEntity = new TextPart(isHtml ? TextFormat.Html : TextFormat.Plain)
        {
            Text = isHtml ? body : $"{originalSender}{Environment.NewLine}{Environment.NewLine}{body}"
        };
        return new MimeMessage(new InternetAddress[] { new MailboxAddress(Settings.SmtpEmailAddress) },
                           to.Select(x => CreateMailboxAddress(x) as InternetAddress),
                           subject, mimeEntity);
    }

    private static MailboxAddress CreateMailboxAddress(EmailAccount from)
    {
        return from.HasAccountName ? new MailboxAddress(from.Name, from.Address) : new MailboxAddress(from.Address);
    }

    public Task<ProcessResult> SendMessagesAsync(EmailAccount from, EmailAccount to, string subject, string body, bool isHtml = false)
    {
        return SendMessagesAsync(from, new EmailAccount[] { to }, subject, body, isHtml);
    }
}