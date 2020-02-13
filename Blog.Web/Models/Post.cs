using System.Collections.Generic;
using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class Post : EntityBase
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
