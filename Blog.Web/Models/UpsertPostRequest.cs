using Blog.Web.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WaterHub.Core;

namespace Blog.Web.Models
{
    public class UpsertPostRequest : IPostInfo
    {
        public string Abstract { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsPublished { get; set; }
        public bool IsSticky { get; set; }
        public Guid Key { get; set; }
        public List<string> Tags { get; set; }

        [Required]
        public string Title { get; set; }

        public string UrlFriendlyTitle { get; set; }

        public Post ToPost()
        {
            var post = new Post().EnsureValidKey(Key);
            post.IsPublished = IsPublished;
            post.Content = Content;
            post.Title = Title;
            post.IsSticky = IsSticky;
            post.Tags = Tags;
            post.Abstract = Abstract;
            return post;
        }
    }
}