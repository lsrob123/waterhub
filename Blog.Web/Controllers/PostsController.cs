using Blog.Web.Abstractions;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using WaterHub.Core.Models;

namespace Blog.Web.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public PostsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        [Route("/info/latest")]
        public IActionResult ListLatestPostInfoEntries()
        {
            var result = _blogService.ListLatestPostInfoEntries();
            return Ok(result);
        }

        //[Authorize(Roles = UserModelBase.Admin)]
        [HttpGet]
        [Route("info/latest/all")]
        public IActionResult ListLatestPostInfoEntriesIncludingUnPublished()
        {
            var result = _blogService.ListLatestPostInfoEntries(includeUnpublishedPosts: true);
            return Ok(result);
        }

        [HttpGet]
        [Route("latest")]
        public IActionResult ListLatestPostsAll()
        {
            var result = _blogService.ListLatestPosts(includeUnpublishedPosts: true);
            return Ok(result);
        }

        [HttpGet]
        [Route("info")]
        public IActionResult ListPostInfoEntriesByKeywords([FromQuery]string keywords)
        {
            if (string.IsNullOrWhiteSpace(keywords))
                return Ok(new Post[] { });

            var keywordList = keywords.Split(new string[] { ",", " " }, System.StringSplitOptions.RemoveEmptyEntries);

            var result = _blogService.ListPostInfoEntriesByKeywords(keywordList);
            return Ok(result);
        }

        //[Authorize(Roles = UserModelBase.Admin)]
        [HttpGet]
        [Route("info/all")]
        public IActionResult ListPostInfoEntriesByKeywordsIncludingUnPublished([FromQuery]string keywords)
        {
            if (string.IsNullOrWhiteSpace(keywords))
                return Ok(new Post[] { });

            var keywordList = keywords.Split(new string[] { ",", " " }, System.StringSplitOptions.RemoveEmptyEntries);

            var result = _blogService.ListPostInfoEntriesByKeywords(keywordList, includeUnpublishedPosts: true);
            return Ok(result);
        }

        //[Authorize(Roles = UserModelBase.Admin)]
        [HttpPost]
        [HttpPut]
        public IActionResult UpsertPost([FromBody]UpsertPostRequest request)
        {
            var result = _blogService.UpsertPost(request.ToPost());
            if (result.IsOk)
                return Ok(result.Data);
            return StatusCode((int)result.Status, result.ErrorMessage);
        }
    }
}