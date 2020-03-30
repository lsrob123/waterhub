namespace WaterHub.Core.Models
{
    public class SmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpEmailAddress { get; set; }

        public bool HasSmtpSettings => !string.IsNullOrWhiteSpace(SmtpUsername) &&
            !string.IsNullOrWhiteSpace(SmtpPassword);
    }
}
