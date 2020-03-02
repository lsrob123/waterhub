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
        private readonly ISerializationService _serializationService;

        public AdminModel(ILogger<AdminModel> logger, IAuthService authService, ITextMapService textMapService,
            IBlogService blogService, ISerializationService serializationService)
          : base(logger, authService, textMapService)
        {
            _blogService = blogService;
            _serializationService = serializationService;
        }

        public string AllTagsInText { get; set; }
        public string ErrorMessage { get; set; }
        public string PostImagesInText { get; set; }

        [BindProperty]
        public Guid? ExistingPostKey { get; set; }

        [BindProperty]
        public string ExistingPostUrlFriendlyTitle { get; set; }

        public bool IsExistingPost => ExistingPostKey.HasValue;
        public override string PageName => PageDefinitions.Admin.PageName;
        public override string PageTitle => PageDefinitions.Admin.PageTitle;

        [BindProperty]
        public Post PostInEdit { get; set; }

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
                ExistingPostUrlFriendlyTitle = PostInEdit.UrlFriendlyTitle;
                SubmitButtonText = "Update";
                AllTagsInText = _serializationService
                    .Serialize(response.AllTags is null ? new string[] { } : response.AllTags);
                PostImagesInText = _serializationService.Serialize(PostInEdit.Images ?? new List<PostImage>());
            }
        }

        public async Task<IActionResult> OnPostUploadImagesAsync(ICollection<IFormFile> files)
        {
            if (!IsExistingPost)
                return RefirectBackToAdminMain();

            await _blogService.SaveUploadImagesAsync(ExistingPostKey.Value, files);
            return RefirectBackToAdminMain();
        }

        private IActionResult RefirectBackToAdminMain()
        {
            return RedirectToPage(PageName, new { title = ExistingPostUrlFriendlyTitle });
        }
    }
}