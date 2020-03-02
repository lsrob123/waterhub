using Blog.Web.Abstractions;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Blog.Web.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostImageController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly ISettings _settings;

        public PostImageController(ISettings settings, IBlogService blogService)
        {
            _settings = settings;
            _blogService = blogService;
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
                return Ok();

            return StatusCode((int)upsertPostResult.Status, upsertPostResult.ErrorMessage);
        }

        [HttpGet]
        [Route("{urlFriendlyTitle}/images/{imageName}")]
        public IActionResult GetImage([FromRoute]string urlFriendlyTitle, [FromRoute]string imageName)
        {
            var result = _blogService.GetPostImagePaths(urlFriendlyTitle, imageName);
            if (!result.IsOk)
            {
                return StatusCode((int)result.Status, result.ErrorMessage);
            }

            return PhysicalFile(result.PostImageFilePath, "image/jpeg");
        }
    }
}