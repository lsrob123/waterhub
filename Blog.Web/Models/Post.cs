﻿using Blog.Web.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using WaterHub.Core;
using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class Post : EntityBase, IPostInfo
    {
        public string Abstract { get; set; }

        [Required]
        public string Content { get; set; }

        public int DayCreated => TimeCreated.Day;
        public bool HasTags => Tags != null && Tags.Any();
        public bool IsPublished { get; set; } = true;
        public bool IsSticky { get; set; }
        public string MonthCreated => $"{TimeCreated:yyyy.MM}";
        public ICollection<string> Tags { get; set; }

        public string TagsInText => JsonSerializer.Serialize(
            (Tags is null) ? new string[] { } : Tags,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        [Required]
        public string Title { get; set; }

        public string UrlFriendlyTitle { get; set; }

        public PostInfoEntry AsPostInfoEntry()
        {
            return new PostInfoEntry(this);
        }

        public ICollection<Tag> AsTagEntities()
        {
            var tags = Tags?.Select(x => new Tag
            {
                PostKey = Key,
                Text = x
            }.EnsureValidKey())?.ToList()
            ?? new List<Tag>();
            return tags;
        }

        public Post BuildUrlFriendlyTitle()
        {
            UrlFriendlyTitle = string.IsNullOrWhiteSpace(Title)
                ? Key.ToString()
                : Title.ToUrlFriendlyString();
            return this;
        }
    }
}