using System;
using System.Collections.Generic;
using Blog.Web.Abstractions;
using Blog.Web.Config;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;

namespace Blog.Web
{
    public class PostsModel : BlodPageModelBase<AdminModel>
    {
        private static readonly string CacheKeyForMorePosts = $"{nameof(PostsModel)}.{nameof(CacheKeyForMorePosts)}";

        private readonly IBlogService _blogService;
        private readonly IMemoryCache _cache;

        public PostsModel(ILogger<AdminModel> logger, IAuthService authService, ITextMapService textMapServic, IBlogService blogService,
            IMemoryCache cache)
            : base(logger, authService, textMapServic)
        {
            _blogService = blogService;
            _cache = cache;
        }

        public override string PageName => PageDefinitions.Posts.PageName;

        public override string PageTitle => string.IsNullOrWhiteSpace(PostInDisplay?.UrlFriendlyTitle)
            ? string.Empty
            : PostInDisplay.Title.Trim();

        public Post PostInDisplay { get; set; }

        public ICollection<PostInfoEntry> MorePosts { get; set; }

        public IActionResult OnGet([FromRoute]string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return NotFound();

            var response = _blogService.GetPostByUrlFriendlyTitle(title);
            PostInDisplay = response?.Post;
            if (PostInDisplay is null || (!IsLoggedIn && !PostInDisplay.IsPublished))
                return NotFound();

            MorePosts = GetMorePosts();

            return Page();
        }

        private ICollection<PostInfoEntry> GetMorePosts()
        {
            if (_cache.TryGetValue<ICollection<PostInfoEntry>>(CacheKeyForMorePosts, out var posts))
            {
                return posts;
            }

            posts = _blogService.ListLatestPostInfoEntries(10);
            _cache.Set(CacheKeyForMorePosts, posts, TimeSpan.FromMinutes(10));
            return posts;
        }
    }
}