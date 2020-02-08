using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;
using WaterHub.Core.Services;

namespace WaterHub.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddWaterHubCoreServices<TSettings>(this IServiceCollection services)
            where TSettings : IHashedPasswordQuery, IHasTextMapFilePath, IHasLiteDbDatabaseName, IHasSerilogSettings
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHashedPasswordQuery>(x => x.GetRequiredService<TSettings>());
            services.AddSingleton<IHasSerilogSettings>(x => x.GetRequiredService<TSettings>());
            services.AddSingleton<IHasTextMapFilePath>(x => x.GetRequiredService<TSettings>());
            services.AddSingleton<IHasLiteDbDatabaseName>(x => x.GetRequiredService<TSettings>());
            services.AddSingleton<IPasswordHasher<UserModelForPasswordProcesses>, PasswordHasher<UserModelForPasswordProcesses>>();
            services.AddSingleton<ITextMapService, TextMapService>();
            services.AddSingleton<IImageProcessService, ImageProcessService>();
            services.AddSingleton<IAuthService, AuthService>();

            var settings = services.BuildServiceProvider().GetRequiredService<TSettings>();
            Log.Logger = new LoggerConfiguration()
                         .Enrich.FromLogContext()
                         .WriteTo.Console(settings.SerilogSettings.LogEventLevel)
                         .WriteTo.File(
                             settings.SerilogSettings.LogFilePath,
                             settings.SerilogSettings.LogEventLevel,
                             rollingInterval: settings.SerilogSettings.RollingInterval)
                         .CreateLogger();

            return services;
        }

    }
}