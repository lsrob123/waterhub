using Serilog;
using Serilog.Events;
using System.IO;

namespace WaterHub.Core.Models
{
    public class SerilogSettings
    {
        public LogEventLevel LogEventLevel { get; set; }
        public string LogFilePath { get; set; }
        public RollingInterval RollingInterval { get; set; }

        public static SerilogSettings CreateDefaultSettings(bool isDevelopmentEnvironment)
        {
            return new SerilogSettings
            {
                LogEventLevel = isDevelopmentEnvironment ? LogEventLevel.Debug : LogEventLevel.Error,
                LogFilePath = Path.Combine(Directory.GetCurrentDirectory(), "logs", "log.txt"),
                RollingInterval = RollingInterval.Day
            };
        }
    }
}