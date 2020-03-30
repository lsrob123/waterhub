using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

namespace Blog.Web.Abstractions
{
    public interface ISettings :IHasTextMapFilePath, IHasLiteDbDatabaseName, IHasSerilogSettings, IHasSmtpSettings
    {
        string AdminHashedPassword { get; }
        int LatestPostsCount { get; }
        int PostsFromSearchCount { get; }
        string UploadImageRootPath { get; }
        int ThumbHeight { get; }
        EmailAccount VasayoEmailAccount { get; }
        EmailAccount SupportEmailAccount { get; }
    }
}