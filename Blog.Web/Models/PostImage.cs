using Blog.Web.Config;
using System.ComponentModel.DataAnnotations;
using System.IO;
using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class PostImage : EntityBase
    {
        public string AppliedName => string.IsNullOrWhiteSpace(Name) ? InternalId.ToString() : Name;

        [Required]
        public string Extension { get; set; }

        public string FilePath => $"{Key}{Extension}";
        public int InternalId { get; set; }
        public string Name { get; set; }
        public string ThumbPath => Path.Combine(Constants.Thumbs, FilePath);
        public string DisplayName => $"{InternalId}{Extension}";
    }
}