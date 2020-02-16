using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public string TagsInText =>
             JsonSerializer.Serialize(
                 (Tags is null)
                 ? new string[] { }
                 : Tags.Select(x => x.Text),
                 new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });


        public Post BuildUrlFriendlyTitle()
        {
            UrlFriendlyTitle = string.IsNullOrWhiteSpace(Title)
                ? Key.ToString()
                : Title.ToUrlFriendlyString();
            return this;
        }
    }
}