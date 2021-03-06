﻿using System.Collections.Generic;
using System.Linq;
using Blog.Web.Abstractions;
using Blog.Web.Config;
using Blog.Web.Models;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;

namespace Blog.Web.Pages
{
    public class IndexModel : BlodPageModelBase<IndexModel>
    {
        private readonly IBlogService _blogService;

        public IndexModel(ILogger<IndexModel> logger, IAuthService authService, ITextMapService textMapService,
            IBlogService blogService)
            : base(logger, authService, textMapService)
        {
            _blogService = blogService;
        }

        public override string PageName => PageDefinitions.Home.PageName;

        public override string PageTitle => PageDefinitions.Home.PageTitle;

        public ICollection<Post> LatestPosts;
        public ICollection<Post> StickyPosts;

        public void OnGet()
        {
            LatestPosts = _blogService.ListLatestPosts();
            StickyPosts = _blogService.ListStickyPosts();

            if (!IsLoggedIn)
            {
                LatestPosts = LatestPosts.Where(x => x.IsPublished).ToList();
                StickyPosts = StickyPosts.Where(x => x.IsPublished).ToList();
            }
        }
    }
}