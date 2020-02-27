using System.ComponentModel.DataAnnotations;
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
    }
}
