using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using WaterHub.Core;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

public class SmtpService
{
    protected readonly ILogger<SmtpService> Logger;
    protected readonly SmtpSettings Settings;

    public SmtpService(IHasSmtpSettings hasSmtpSettings, ILogger<SmtpService> logger)
    {
        Settings = hasSmtpSettings.SmtpSettings;
        Logger = logger;
    }

    public virtual ProcessResult SendMessages(EmailContact from, IEnumerable<EmailContact> to, string subject, string body,
        bool isHtml = false)
    {
        //var receipients=to.Select()

        var messages = to.Select(x => new MimeMessage(
            new InternetAddress[] { new MailboxAddress(from.Name, from.Address) },
            to.Select(x => new MailboxAddress(x.Name, x.Address)),
            subject)
        {
            Body = new TextPart(isHtml ? TextFormat.Html : TextFormat.Plain)
            {
                Text = body
            }
        }
        ).ToList();

        var exceptions = new List<Exception>();

        using var client = new SmtpClient();
        try
        {
            client.Connect(Settings.Host, Settings.Port, SecureSocketOptions.SslOnConnect);

            client.Authenticate(Settings.Username, Settings.Password);

            foreach (var message in messages)
            {
                try
                {
                    client.Send(message);
                }
                catch (Exception individualSendException)
                {
                    exceptions.Add(individualSendException);
                }
            }

            client.Disconnect(true);

            if (!exceptions.Any())
                return new ProcessResult().AsOk();

            return Logger.LogAndReturnProcessResult(new AggregateException(exceptions));
        }
        catch (Exception exception)
        {
            return Logger.LogAndReturnProcessResult(exception);
        }
    }
}