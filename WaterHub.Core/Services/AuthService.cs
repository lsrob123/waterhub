using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

namespace WaterHub.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;
        private readonly IPasswordHasher<UserModelBase> _passwordHasher;
        private readonly IUserQuery _userQuery;

        public AuthService(IPasswordHasher<UserModelBase> passwordHasher, IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory, IUserQuery userQuery)
        {
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
            _userQuery = userQuery;
            _logger = loggerFactory.CreateLogger(GetType().FullName);
        }

        public string GetUserIdentityName()
        {
            if (!IsLoggedIn())
                return null;

            return _httpContextAccessor.HttpContext.User.Identity.Name ?? "unnamed";
        }

        public string HashPassword(string plainTextPassword)
        {
            var userModel = new UserModelBase { PlainTextPassword = plainTextPassword };
            var hash = _passwordHasher.HashPassword(userModel, plainTextPassword);
            return hash;
        }

        public bool IsAdmin()
        {
            if (!IsLoggedIn())
                return false;

            var user = _httpContextAccessor.HttpContext.User.ToUserModel<UserModelBase>();
            return user?.IsAdmin == true;
        }

        public bool IsLoggedIn()
        {
            return !(_httpContextAccessor.HttpContext.User is null) &&
                !(_httpContextAccessor.HttpContext.User.Identity is null) &&
                _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public async Task<ProcessResult> SignInAsync(string username, string plainTextPassword)
        {
            try
            {
                var user = VerifyHashedPassword(username, plainTextPassword);
                if (user is null)
                    return new ProcessResult().AsError(HttpStatusCode.Unauthorized);
                user.Username ??= username;

                var properties = new AuthenticationProperties { IsPersistent = false };
                await _httpContextAccessor.HttpContext
                    .SignInAsync(user.ToClaimsPrincipal(CookieAuthenticationDefaults.AuthenticationScheme), properties);
                return new ProcessResult().AsOk();
            }
            catch (Exception ex)
            {
                var errorCode = Guid.NewGuid();
                _logger.LogError(ex, ex.CodedErrorMessage(errorCode));
                await SignOutAsync();
                return new ProcessResult().AsError(ex, errorCode.ToString(), HttpStatusCode.InternalServerError);
            }
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public UserModelBase VerifyHashedPassword(string username, string plainTextPassword)
        {
            var user = _userQuery.GetUser(username);
            if ((user is null) || string.IsNullOrWhiteSpace(user.HashedPassword))
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, plainTextPassword);
            return result == PasswordVerificationResult.Success ? user : null;
        }
    }
}