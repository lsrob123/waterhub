using System;
using System.Collections.Generic;
using System.Linq;
using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class PostInfoEntry : EntityBase
    {
        public PostInfoEntry()
        {
        }

        public PostInfoEntry(Post post)
        {
            PostKey = post.Key;
            IsPublished = post.IsPublished;
            IsSticky = post.IsSticky;
            Title = post.Title;
            UrlFriendlyTitle = post.UrlFriendlyTitle;
            TimeCreated = post.TimeCreated;
            TimeUpdated = post.TimeUpdated;
            Tags = post.Tags.Select(x => x.Text).ToList();
        }

        public Guid PostKey { get; set; }
        public bool IsPublished { get; set; }
        public bool IsSticky { get; set; }
        public ICollection<string> Tags { get; set; }
        public string Title { get; set; }
        public string UrlFriendlyTitle { get; set; }
    }
}
