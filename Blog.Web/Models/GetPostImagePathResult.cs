using System.Net;
using WaterHub.Core;
using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class GetPostImagePathResult : ProcessResult<string>
    {
        public string PostImageFilePath => Data;
        public string ThumbnailFilePath { get; set; }

        public GetPostImagePathResult AsError(HttpStatusCode status, string message)
        {
            return this.AsError<GetPostImagePathResult, string>(status, null, message);
        }

        public GetPostImagePathResult AsOk(string postImageFilePath, string thumbnailFilePath)
        {
            ThumbnailFilePath = thumbnailFilePath;
            return this.AsOk(postImageFilePath);
        }
    }
}