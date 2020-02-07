using WaterHub.Core.Abstractions;

namespace Gallery.Web.Abstractions
{
    public interface ISettings: IHashedPasswordQuery, IHasTextMapFilePath
    {
        string AlbumRootPath { get; }
        string Database { get; }
        string DefaultThumbnailUriPathForAlbum { get; }
        string HashedPassword { get; }
        int UploadImageIconHeight { get; }
        string UploadImageRootPath { get; }
        int UploadImageThumbnailHeight { get; }
        int DaysOfAlbumsDisplayed { get; }
        int AlbumCountMax { get; }
    }
}