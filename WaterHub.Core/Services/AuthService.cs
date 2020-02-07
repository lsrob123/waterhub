using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

namespace WaterHub.Core.Services
{
    public class AuthService: IAuthService
    {
        private readonly IPasswordHasher<UserModelForPasswordProcesses> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHashedPasswordQuery _hashedPasswordQuery;
        private readonly ILogger _logger;
        

        public AuthService(IPasswordHasher<UserModelForPasswordProcesses> passwordHasher, IHttpContextAccessor httpContextAccessor,
            ILoggerFactory loggerFactory, IHashedPasswordQuery hashedPasswordQuery)
        {
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
            _hashedPasswordQuery = hashedPasswordQuery;
            _logger = loggerFactory.CreateLogger(GetType().FullName);
        }

        public bool VerifyHashedPassword(string username, string plainTextPassword)
        {
            var hashedPassword = _hashedPasswordQuery.GetHashedPassword(username);
            var userModel =  new UserModelForPasswordProcesses { PlainTextPassword = plainTextPassword };
            var result = _passwordHasher.VerifyHashedPassword(userModel, hashedPassword, plainTextPassword);
            return result == PasswordVerificationResult.Success;
        }

        public string HashPassword(string plainTextPassword)
        {
            var userModel = new UserModelForPasswordProcesses { PlainTextPassword = plainTextPassword };
            var hash = _passwordHasher.HashPassword(userModel, plainTextPassword);
            return hash;
        }

        public async Task<bool> SignInAsync(string username, string plainTextPassword)
        {
            try
            {
                var loginSucceeded = VerifyHashedPassword(username, plainTextPassword);
                if (!loginSucceeded)
                    throw new Exception("Wrong password");

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, username));

                var principle = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties { IsPersistent = false };
                await _httpContextAccessor.HttpContext.SignInAsync(principle, properties);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await SignOutAsync();
                return false;
            }
        }

        public async Task SignOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public bool IsLoggedIn()
        {
            return !(_httpContextAccessor.HttpContext.User is null) &&
                !(_httpContextAccessor.HttpContext.User.Identity is null) &&
                _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public string GetUserIdentityName()
        {
            if (!IsLoggedIn())
                return null;

            return _httpContextAccessor.HttpContext.User.Identity.Name ?? "unnamed";
        }
    }
}

