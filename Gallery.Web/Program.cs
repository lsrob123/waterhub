using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics;
using System.Linq;
using WaterHub.Core;

namespace Gallery.Web
{
    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);

            if (!(Debugger.IsAttached || args.Contains("--console")))
                hostBuilder = hostBuilder.UseWindowsService();

            return hostBuilder
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseWebHostSettings();
                });
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}