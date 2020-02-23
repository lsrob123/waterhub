using Blog.Web.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class PostInfoEntry : EntityBase, IPostInfo
    {
        public PostInfoEntry()
        {
        }

        public PostInfoEntry(Post post)
        {
            Key = post.Key;
            IsPublished = post.IsPublished;
            IsSticky = post.IsSticky;
            Title = post.Title;
            UrlFriendlyTitle = post.UrlFriendlyTitle;
            TimeCreated = post.TimeCreated;
            TimeUpdated = post.TimeUpdated;
            Tags = post.Tags;
            Abstract = post.Abstract;
        }

        public string Abstract { get; set; }

        public bool IsPublished { get; set; }
        public bool IsSticky { get; set; }
        public ICollection<string> Tags { get; set; }

        [Required]
        public string Title { get; set; }

        public string UrlFriendlyTitle { get; set; }
    }
}