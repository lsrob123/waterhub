using System.ComponentModel.DataAnnotations;
using System.IO;
using Blog.Web.Config;
using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class PostImage : EntityBase
    {
        public int InternalId { get; set; }
        public string Name { get; set; }

        [Required]
        public string Extension { get; set; }
        public string AppliedName => string.IsNullOrWhiteSpace(Name) ? InternalId.ToString() : Name;

        public string FilePath => $"{Key}{Extension}";
        public string ThumbPath => Path.Combine(Constants.Thumbs, FilePath);
    }
}
