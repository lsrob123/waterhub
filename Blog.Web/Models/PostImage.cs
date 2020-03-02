using System.ComponentModel.DataAnnotations;
using System.IO;
using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class PostImage : EntityBase
    {
        public string DisplayName => $"{InternalId}{Extension}";

        [Required]
        public string Extension { get; set; }

        public string FilePath => $"{Key}{Extension}";
        public int InternalId { get; set; }
        public string ThumbPath => Path.Combine(Config.Constants.Thumbs, FilePath);
    }
}