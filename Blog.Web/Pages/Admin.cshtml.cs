using Blog.Web.Abstractions;
using Blog.Web.Config;
using Blog.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using WaterHub.Core;
using WaterHub.Core.Abstractions;

namespace Blog.Web
{
    public class AdminModel : BlodPageModelBase<AdminModel>
    {
        private readonly IBlogService _blogService;

        public AdminModel(ILogger<AdminModel> logger, IAuthService authService, ITextMapService textMapService,
            IBlogService blogService)
          : base(logger, authService, textMapService)
        {
            _blogService = blogService;
        }

        public string ErrorMessage { get; set; }
        public override string PageName => PageDefinitions.Admin.PageName;
        public override string PageTitle => PageDefinitions.Admin.PageTitle;

        public Guid? ExistingPostKey { get; set; }
        public bool IsExistingPost => ExistingPostKey.HasValue;

        [BindProperty]
        public Post PostInEdit { get; set; }

        public string AllTagsInText { get; set; }

        public string SubmitButtonText { get; private set; }

        public void OnGet([FromRoute]string title)
        {
            ExistingPostKey = null;
            if (string.IsNullOrWhiteSpace(title))
            {
                PostInEdit = new Post().EnsureValidKey();
                SubmitButtonText = "Publish";
                return;
            }

            var response = _blogService.GetPostByUrlFriendlyTitle(title);
            PostInEdit = response?.Post;

            if (PostInEdit == null)
            {
                SubmitButtonText = "Publish";
                PostInEdit = new Post().EnsureValidKey();
            }
            else
            {
                ExistingPostKey = PostInEdit.Key;
                SubmitButtonText = "Update";
                AllTagsInText = JsonSerializer.Serialize(
                    response.AllTags is null ? new string[] { } : response.AllTags,
                    new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
        }

        public async Task<IActionResult> OnPostUploadImagesAsync(ICollection<IFormFile> files)
        {
            if (!IsExistingPost)
                return Page();

            await _blogService.SaveUploadImagesAsync(PostInEdit.Key, files);
            return RedirectToPage(PageName, new { title = PostInEdit.UrlFriendlyTitle });
        }

    }
}