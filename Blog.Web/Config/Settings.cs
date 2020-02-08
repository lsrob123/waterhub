using Blog.Web.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WaterHub.Core.Models;

namespace Blog.Web.Config
{
    public class Settings : ISettings
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public Settings(IWebHostEnvironment env, IConfiguration configuration)
        {
            _configuration = configuration;
            _env = env;
        }

        public string HashedPassword => _configuration.GetValue<string>(nameof(HashedPassword));
        public string LiteDbDatabaseName => _configuration.GetValue<string>(nameof(LiteDbDatabaseName));

        public SerilogSettings SerilogSettings => SerilogSettings.CreateDefaultSettings(_env.IsDevelopment());

        public string TextMapFilePath => _configuration.GetValue<string>(nameof(TextMapFilePath));

        public string GetHashedPassword(string username)
        {
            return HashedPassword;
        }
    }
}