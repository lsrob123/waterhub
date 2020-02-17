using Blog.Web.Abstractions;
using Blog.Web.Config;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;

namespace Blog.Web
{
    public class PostsModel : BlodPageModelBase<EditModel>
    {
        private readonly IBlogService _blogService;

        public PostsModel(ILogger<EditModel> logger, IAuthService authService, ITextMapService textMapServic, IBlogService blogService)
            : base(logger, authService, textMapServic)
        {
            _blogService = blogService;
        }

        public override string PageName => PageDefinitions.Posts.PageName;

        public override string PageTitle => $"{T.GetMap(PageDefinitions.Posts.PageTitle, PageDefinitions.Posts.Context)} {PostTitleInPageTitle}";

        public Post PostInDisplay { get; set; }

        private string PostTitleInPageTitle => string.IsNullOrWhiteSpace(PostInDisplay?.UrlFriendlyTitle)
            ? string.Empty
            : $" - {PostInDisplay?.UrlFriendlyTitle.Trim()}";

        public IActionResult OnGet([FromRoute]string article)
        {
            if (string.IsNullOrWhiteSpace(article))
                return NotFound();



            var response = _blogService.GetPostByUrlFriendlyTitle(article);
            PostInDisplay = response?.Post;
            if (PostInDisplay is null || (!IsLoggedIn && !PostInDisplay.IsPublished))
                return NotFound();

            return Page();
        }
    }
}