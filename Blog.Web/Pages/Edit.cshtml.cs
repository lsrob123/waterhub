using System;
using System.Threading.Tasks;
using Blog.Web.Abstractions;
using Blog.Web.Config;
using Blog.Web.Models;
using Microsoft.Extensions.Logging;
using WaterHub.Core;
using WaterHub.Core.Abstractions;

namespace Blog.Web
{
    public class EditModel : BlodPageModelBase<EditModel>
    {
        private readonly IBlogService _blogService;

        public EditModel(ILogger<EditModel> logger, IAuthService authService, ITextMapService textMapService, IBlogService blogService)
          : base(logger, authService, textMapService)
        {
            _blogService = blogService;
        }

        public override string PageName => PageDefinitions.Edit.PageName;

        public override string PageTitle => PageDefinitions.Edit.PageTitle;

        public Post PostInEdit { get; set; }

        public void OnGet(Guid? postKey)
        {
            if (postKey.HasValue)
            {
                var response = _blogService.GetPostByKey(postKey.Value);
                PostInEdit = response?.Post ?? new Post().EnsureValidKey();
            }
            else
            {
                PostInEdit = new Post().EnsureValidKey();
            }
        }

        public Task OnPostArticleAsync(Guid postKey, string title, string content, string flagText)
    }
}