using Gallery.Web.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WaterHub.Core.Models;

namespace Gallery.Web.Config
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

        public int AlbumCountMax => _configuration.GetValue<int>(nameof(AlbumCountMax));
        public string AlbumRootPath => _configuration.GetValue<string>(nameof(AlbumRootPath));
        public int DaysOfAlbumsDisplayed => _configuration.GetValue<int>(nameof(DaysOfAlbumsDisplayed));

        public string DefaultThumbnailUriPathForAlbum => _configuration
            .GetValue<string>(nameof(DefaultThumbnailUriPathForAlbum));

        public string HashedPassword => _configuration.GetValue<string>(nameof(HashedPassword));
        public string LiteDbDatabaseName => _configuration.GetValue<string>(nameof(LiteDbDatabaseName));

        public SerilogSettings SerilogSettings => SerilogSettings.CreateDefaultSettings(_env.IsDevelopment());

        public string TextMapFilePath => _configuration.GetValue<string>(nameof(TextMapFilePath));
        public int UploadImageIconHeight => _configuration.GetValue<int>(nameof(UploadImageIconHeight));
        public string UploadImageRootPath => _configuration.GetValue<string>(nameof(UploadImageRootPath));
        public int UploadImageThumbnailHeight => _configuration.GetValue<int>(nameof(UploadImageThumbnailHeight));

        public string GetHashedPassword(string username)
        {
            return HashedPassword;
        }
    }
}