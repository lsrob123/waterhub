using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Serilog;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;
using WaterHub.Core.Services;

namespace WaterHub.Core
{
    public static class Extensions
    {
        public static string LogAndReturnErrorCode<T>(this ILogger<T> logger, Exception e, string message=null)
            where T:class
        {
            var errorCode = Guid.NewGuid().ToString();
            message = string.IsNullOrWhiteSpace(message) ? e.Message : message;
            logger.LogError(exception: e, $"[{errorCode}] {message}");
            return errorCode;
        }

        public static ProcessResult LogAndReturnProcessResult<T>(this ILogger<T> logger, Exception e, string message = null)
            where T : class
        {
            var errorCode = logger.LogAndReturnErrorCode(e, message);
            return new ProcessResult()
                .AsError(HttpStatusCode.InternalServerError, message: message, errorCodeInLog: errorCode);
        }

        public static IServiceCollection AddSmtpService<TSettings>(this IServiceCollection services)
            where TSettings: IHasSmtpSettings
        {
            services.AddSingleton<IHasSmtpSettings>(x => x.GetRequiredService<TSettings>());
            return services;
        }

        public static IServiceCollection AddWaterHubCoreServices<TSettings, THashedPasswordQuery>
            (this IServiceCollection services)
            where TSettings : IHasTextMapFilePath, IHasLiteDbDatabaseName, IHasSerilogSettings
            where THashedPasswordQuery : IUserQuery
        {
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ISerializationService, SerializationService>();
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

        public static TProcessResult AsError<TProcessResult, TData>(this TProcessResult result, HttpStatusCode status,
            TData data = default, string message = null, string errorCodeInLog = null)
            where TProcessResult : ProcessResult<TData>
        {
            result.AsError(new Exception(message), errorCodeInLog, status, data);
            return result;
        }

        public static TProcessResult AsError<TProcessResult, TData>(this TProcessResult result, Exception exception,
            string errorCodeInLog, HttpStatusCode status, TData data = default)
            where TProcessResult : ProcessResult<TData>
        {
            result.Data = data;
            result.Status = status;
            result.Errors = new List<Exception> { exception };
            result.ErrorCodeInLog = errorCodeInLog;
            return result;
        }

        public static ProcessResult<TData> AsError<TData>(this ProcessResult<TData> result, HttpStatusCode status,
          TData data = default, string message = null, string errorCodeInLog = null)
        {
            return result.AsError<ProcessResult<TData>, TData>(new Exception(message), errorCodeInLog, status, data);
        }

        public static ProcessResult<TData> AsError<TData>(this ProcessResult<TData> result, Exception exception,
            string errorCodeInLog, HttpStatusCode status, TData data = default)
        {
            return result.AsError<ProcessResult<TData>, TData>(exception, errorCodeInLog, status, data);
        }

        public static TProcessResult AsErrors<TProcessResult, TData>(this TProcessResult result,
                            IEnumerable<Exception> exceptions, string errorCodeInLog, HttpStatusCode status, TData data = default)
            where TProcessResult : ProcessResult<TData>
        {
            result.Data = data;
            result.Status = status;
            result.Errors = exceptions.ToList();
            result.ErrorCodeInLog = errorCodeInLog;
            return result;
        }

        public static ProcessResult<TData> AsErrors<TData>(this ProcessResult<TData> result,
            IEnumerable<Exception> exceptions, string errorCodeInLog, HttpStatusCode status, TData data = default)
        {
            return result.AsErrors<ProcessResult<TData>, TData>(exceptions, errorCodeInLog, status, data);
        }

        public static TProcessResult AsOk<TProcessResult, TData>(this TProcessResult result, TData data = default)
                    where TProcessResult : ProcessResult<TData>
        {
            result.Data = data;
            result.Status = HttpStatusCode.OK;
            return result;
        }

        public static ProcessResult<TData> AsOk<TData>(this ProcessResult<TData> result, TData data = default)
        {
            return result.AsOk<ProcessResult<TData>, TData>(data);
        }

        public static string CodedErrorMessage(this Exception exception, Guid? code = null)
        {
            code ??= Guid.NewGuid();
            return $"[{code}]{exception.Message}";
        }

        public static T EnsureValidKey<T>(this T entity, Guid? key = null, bool enforceValueFromArgument = false)
            where T : EntityBase
        {
            key = key.HasValue && key.Value != default ? key : Guid.NewGuid();

            if (entity.Key == default || enforceValueFromArgument)
                entity.Key = key.Value;
            return entity;
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

        public static IWebHostBuilder UseWebHostSettings(this IWebHostBuilder webBuilder, string fileName = null)
        {
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = string.IsNullOrWhiteSpace(fileName)
                ? Path.Combine(rootPath, "hostsettings.json")
                : Path.Combine(rootPath, fileName);

            var json = File.ReadAllText(filePath);
            var settings = JsonSerializer.Deserialize<HostSettings>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            webBuilder.UseUrls(settings.Url);
            return webBuilder;
        }

        public static T WithUpdateOnTimeUpdated<T>(this T entity)
                  where T : EntityBase
        {
            entity.TimeUpdated = DateTimeOffset.UtcNow;
            return entity;
        }
    }
}