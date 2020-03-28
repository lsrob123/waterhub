using Blog.Web.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WaterHub.Core.Models;

namespace Blog.Web.Config
{
    public class Settings : ISettings
    {
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;

        public Settings(IHostEnvironment env, IConfiguration configuration)
        {
            _configuration = configuration;
            _env = env;
        }

        public string AdminHashedPassword => _configuration.GetValue<string>(nameof(AdminHashedPassword));
        public string LiteDbDatabaseName => _configuration.GetValue<string>(nameof(LiteDbDatabaseName));

        public SerilogSettings SerilogSettings => SerilogSettings.CreateDefaultSettings(_env);

        public string TextMapFilePath => _configuration.GetValue<string>(nameof(TextMapFilePath));

        public int LatestPostsCount => _configuration.GetValue<int>(nameof(LatestPostsCount));

        public int PostsFromSearchCount => _configuration.GetValue<int>(nameof(PostsFromSearchCount));

        public string UploadImageRootPath => _configuration.GetValue<string>(nameof(UploadImageRootPath));

        public int ThumbHeight => _configuration.GetValue<int>(nameof(ThumbHeight));

        public string VasayoEmailAddress => _configuration.GetValue<string>(nameof(VasayoEmailAddress));

        public string SupportEmailAddress => _configuration.GetValue<string>(nameof(SupportEmailAddress));

        public SmtpSettings SmtpSettings => _configuration.GetValue<SmtpSettings>(nameof(SmtpSettings));
    }
}