using WaterHub.Core.Abstractions;

namespace Blog.Web.Abstractions
{
    public interface ISettings :IHasTextMapFilePath, IHasLiteDbDatabaseName, IHasSerilogSettings, IHasSmtpSettings
    {
        string AdminHashedPassword { get; }
        int LatestPostsCount { get; }
        int PostsFromSearchCount { get; }
        string UploadImageRootPath { get; }
        int ThumbHeight { get; }
        string VasayoEmailAddress { get; }
        string SupportEmailAddress { get; }
    }
}