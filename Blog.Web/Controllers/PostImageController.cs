using Blog.Web.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

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

        [Authorize]
        [HttpDelete]
        [Route("{urlFriendlyTitle}/images/{imageKey}")]
        public IActionResult DeleteImage([FromRoute]string urlFriendlyTitle, [FromRoute]Guid imageKey)
        {
            var result = _blogService.DeletePostImage(urlFriendlyTitle, imageKey);
            if (result.IsOk)
                return Ok();

            return StatusCode((int)result.Status, result.ErrorMessage);
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