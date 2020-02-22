using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using System;
using System.Linq;
using System.Security.Claims;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;
using WaterHub.Core.Services;

namespace WaterHub.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddWaterHubCoreServices<TSettings, THashedPasswordQuery>
                            (this IServiceCollection services)
            where TSettings : IHasTextMapFilePath, IHasLiteDbDatabaseName, IHasSerilogSettings
            where THashedPasswordQuery : IUserQuery
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUserQuery>(x => x.GetRequiredService<THashedPasswordQuery>());
            services.AddSingleton<IHasSerilogSettings>(x => x.GetRequiredService<TSettings>());
            services.AddSingleton<IHasTextMapFilePath>(x => x.GetRequiredService<TSettings>());
            services.AddSingleton<IHasLiteDbDatabaseName>(x => x.GetRequiredService<TSettings>());
            services.AddSingleton<IPasswordHasher<UserModelBase>, PasswordHasher<UserModelBase>>();
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

        public static string CodedErrorMessage(this Exception exception, Guid? code = null)
        {
            code ??= Guid.NewGuid();
            return $"[{code}]{exception.Message}";
        }

        public static bool IsAdmin(this ClaimsPrincipal claimsPrincipal)
        {
            var userModel = claimsPrincipal.ToUserModel<UserModelBase>();
            return userModel?.IsAdmin == true;
        }

        public static ClaimsPrincipal ToClaimsPrincipal<TUserModel>(this TUserModel user, string authenticationScheme)
           where TUserModel : class, IUserModelBase, new()
        {
            var identity = new ClaimsIdentity(authenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));

            if (user.IsAdmin)
                identity.AddClaim(new Claim(ClaimTypes.Role, UserModelBase.Admin));
            else
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Username));

            if (!string.IsNullOrEmpty(user.MobilePhone))
                identity.AddClaim(new Claim(ClaimTypes.MobilePhone, user.MobilePhone));

            return new ClaimsPrincipal(identity);
        }

        public static string ToUrlFriendlyString(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new ArgumentNullException(nameof(input));
            return input.Trim().Replace(" ", "-").ToLower();
        }

        public static TUserModel ToUserModel<TUserModel>(this ClaimsPrincipal claimsPrincipal)
                    where TUserModel : class, IUserModelBase, new()
        {
            var username = claimsPrincipal?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var isAdmin = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value == UserModelBase.Admin;
            return new TUserModel
            {
                Username = username,
                IsAdmin = isAdmin,
                Email = isAdmin ? null : username,
                MobilePhone = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.MobilePhone)?.Value
            };
        }

        public static T WithUpdateOnTimeUpdated<T>(this T entity)
          where T : EntityBase
        {
            entity.TimeUpdated = DateTimeOffset.UtcNow;
            return entity;
        }

        public static T EnsureValidKey<T>(this T entity, Guid? key = null) where T : EntityBase
        {
            key = key.HasValue && key.Value != default ? key : Guid.NewGuid();

            if (entity.Key == default)
                entity.Key = key.Value;
            return entity;
        }
    }
}