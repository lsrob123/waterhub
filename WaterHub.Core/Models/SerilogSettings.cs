using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace WaterHub.Core.Models
{
    public class SerilogSettings
    {
        public LogEventLevel LogEventLevel { get; set; }
        public string LogFilePath { get; set; }
        public RollingInterval RollingInterval { get; set; }

        public static SerilogSettings CreateDefaultSettings(IHostEnvironment env)
        {
            return new SerilogSettings
            {
                LogEventLevel = env.IsDevelopment()? LogEventLevel.Debug : LogEventLevel.Error,
                LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "log.txt"),
                RollingInterval = RollingInterval.Day
            };
        }
    }
}