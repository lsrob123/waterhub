using Blog.Web.Abstractions;
using Blog.Web.Config;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using WaterHub.Core;
using WaterHub.Core.Abstractions;

namespace Blog.Web
{
    public class EditModel : BlodPageModelBase<EditModel>
    {
        private readonly IBlogService _blogService;

        public EditModel(ILogger<EditModel> logger, IAuthService authService, ITextMapService textMapService,
            IBlogService blogService)
          : base(logger, authService, textMapService)
        {
            _blogService = blogService;
        }

        public string ErrorMessage { get; set; }
        public override string PageName => PageDefinitions.Edit.PageName;
        public override string PageTitle => PageDefinitions.Edit.PageTitle;

        [BindProperty]
        public Post PostInEdit { get; set; }

        public string AllTagsInText { get; set; }

        public string SubmitButtonText { get; private set; }

        public void OnGet([FromRoute]string article)
        {
            if (string.IsNullOrWhiteSpace(article))
            {
                PostInEdit = new Post().WithValidKey();
                SubmitButtonText = "Publish";
                return;
            }

            var response = _blogService.GetPostByUrlFriendlyTitle(article);
            PostInEdit = response?.Post;

            if (PostInEdit == null)
            {
                SubmitButtonText = "Publish";
                PostInEdit = new Post().WithValidKey();
            }
            else
            {
                SubmitButtonText = "Update";
                AllTagsInText = JsonSerializer.Serialize(
                    response.AllTags is null ? new string[] { } : response.AllTags,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
        }

    }
}