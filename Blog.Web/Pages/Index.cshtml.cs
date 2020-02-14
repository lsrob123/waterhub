using System.Collections.Generic;
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
            var response = _blogService.ListLatestPosts();
            LatestPosts = response.LatestPosts ?? new List<Post>();
            StickyPosts = response.StickyPosts ?? new List<Post>();
        }
    }
}