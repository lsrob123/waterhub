using System.Collections.Generic;
using WaterHub.Core;
using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class Post : EntityBase
    {
        public string Content { get; set; }
        public bool IsSticky { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public string Title { get; set; }
        public string UrlFriendlyTitle { get; set; }

        public Post BuildUrlFriendlyTitle()
        {
            UrlFriendlyTitle = string.IsNullOrWhiteSpace(Title)
                ? Key.ToString()
                : Title.ToUrlFriendlyString();
            return this;
        }
    }
}