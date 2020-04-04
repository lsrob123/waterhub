using WaterHub.Core.Abstractions;

namespace Gallery.Web.Abstractions
{
    public interface ISettings: IUserQuery, IHasTextMapFilePath, IHasLiteDbDatabaseName, IHasSerilogSettings
    {
        string SiteName { get; }
        string DomainName { get; }
        string AlbumRootPath { get; }
        string DefaultThumbnailUriPathForAlbum { get; }
        string AdminHashedPassword { get; }
        int UploadImageIconHeight { get; }
        string UploadImageRootPath { get; }
        int UploadImageThumbnailHeight { get; }
        int DaysOfAlbumsDisplayed { get; }
        int AlbumCountMax { get; }
    }
}