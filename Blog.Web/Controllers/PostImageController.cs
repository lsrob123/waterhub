using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blog.Web.Abstractions;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostImageController : ControllerBase
    {
        private readonly ISettings _settings;
        private readonly IBlogService _blogService;

        public PostImageController(ISettings settings, IBlogService blogService)
        {
            _settings = settings;
            _blogService = blogService;
        }

        [HttpGet]
        [Route("{urlFriendlyTitle}/images/{imageName}")]
        public IActionResult GetImage([FromRoute]string urlFriendlyTitle, [FromRoute]string imageName)
        {
            var postResult = _blogService.GetPostByUrlFriendlyTitle(urlFriendlyTitle);
            if (postResult.Post == null)
                return NotFound("No post found");

            var postImage = postResult.Post.Images.SingleOrDefault(x => x.Value.AppliedName == imageName);
            if (postImage.Value == null)
                return NotFound("No image found in the post");

            var filePath = Path.Combine(_settings.UploadImageRootPath, postImage.Value.FilePath);
            if (!System.IO.File.Exists(filePath))
                return NotFound("No image file found");

            using var image = System.IO.File.OpenRead(filePath);
            return File(image, "image/jpeg");
        }

        [HttpDelete]
        [Route("{urlFriendlyTitle}/images/{imageKey}")]
        public IActionResult DeleteImage([FromRoute]string urlFriendlyTitle, [FromRoute]Guid imageKey)
        {
            var getPostResult = _blogService.GetPostByUrlFriendlyTitle(urlFriendlyTitle);
            if (getPostResult.Post == null)
                return NotFound("No post found");

            getPostResult.Post.DeletePostImage(imageKey);
            var upsertPostResult = _blogService.UpsertPost(getPostResult.Post);
            if (upsertPostResult.IsOk)
                return Ok(upsertPostResult.Data.Images ?? new Dictionary<Guid, PostImage>());

            return StatusCode((int)upsertPostResult.Status, upsertPostResult.ErrorMessage);
        }
    }
}
