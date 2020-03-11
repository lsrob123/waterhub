using WaterHub.Core.Models;

namespace WaterHub.Core.Abstractions
{
    public interface IHasSmtpSettings
    {
        SmtpSettings SmtpSettings { get; }
    }
}
