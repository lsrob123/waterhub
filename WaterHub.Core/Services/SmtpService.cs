using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

public class SmtpService
{
    protected readonly SmtpSettings Settings;
    protected readonly ILogger<SmtpService> Logger;

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
        using (var client = new SmtpClient())
        {
            try
            {
                client.Connect(Settings.Host, Settings.Port, SecureSocketOptions.SslOnConnect);

                client.Authenticate(Settings.Username, Settings.Password);

                foreach (var message in messages)
                {
                    client.Send(message);
                }

                client.Disconnect(true);
            }
            catch (Exception upperLevelException)
            {
                Logger.LogError(exception)
            }
        }
    }
}