using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Gallery.Web.Controllers
{
    [Route("api/album")]
    [ApiController]
    public class AlbumController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly ILogger<AlbumController> _logger;

        public AlbumController(ILogger<AlbumController> logger, IAlbumService albumService)
        {
            _logger = logger;
            _albumService = albumService;
        }

        [Authorize]
        [HttpPut]
        [Route("{albumName}/image/{processedFileName}")]
        public IActionResult UpdateUploadImageDisplayOrder([FromRoute]string albumName,
            [FromRoute]string processedFileName, [FromBody] UpdateUploadImageDisplayOrderRequest request)
        {
            var result = _albumService.UpdateUploadImageDisplayOrder(albumName, processedFileName, request.DisplayOrder);
            return result.Status switch
            {
                HttpStatusCode.NotFound => NotFound(),
                _ => Ok(result.Data.UploadImages[processedFileName]),
            };
        }
    }
}