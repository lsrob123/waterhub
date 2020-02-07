using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;
using WaterHub.Core.Services;

namespace WaterHub.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddWaterHubCoreServices<TSettings>(this IServiceCollection services)
            where TSettings: IHashedPasswordQuery, IHasTextMapFilePath, IHasLiteDbDatabaseName
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHashedPasswordQuery>(x => x.GetRequiredService<TSettings>());
            services.AddSingleton<IHasTextMapFilePath>(x => x.GetRequiredService<TSettings>());
            services.AddSingleton<IHasLiteDbDatabaseName>(x => x.GetRequiredService<TSettings>());
            services.AddSingleton<IPasswordHasher<UserModelForPasswordProcesses>, PasswordHasher<UserModelForPasswordProcesses>>();
            services.AddSingleton<ITextMapService, TextMapService>();
            services.AddSingleton<IImageProcessService, ImageProcessService>();
            services.AddSingleton<IAuthService, AuthService>();

            return services;
        }
    }
}
