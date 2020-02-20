using Blog.Web.Abstractions;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;

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
        [Route("latest/published")]
        public IActionResult ListLatestPostInfoEntries()
        {
            var result = _blogService.ListLatestPostInfoEntries();
            return Ok(result);
        }

        [HttpGet]
        [Route("latest/info")]
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
        public IActionResult ListPostInfoEntriesByKeywordsInTitle([FromQuery]string keywords)
        {
            if (string.IsNullOrWhiteSpace(keywords))
                return Ok(new Post[] { });

            var keywordList = keywords.Split(new string[] { ",", " " }, System.StringSplitOptions.RemoveEmptyEntries);

            var result = _blogService.ListPostInfoEntriesByKeywordsInTitle(keywordList);
            return Ok(result);
        }

        [HttpPost]
        [HttpPut]
        //[Authorize(Roles = UserModelBase.Admin)]
        public IActionResult UpsertPost([FromBody]UpsertPostRequest request)
        {
            var result = _blogService.UpsertPost(request.ToPost());
            if (result.IsOk)
                return Ok(result.Data);
            return StatusCode((int)result.Status, result.ErrorMessage);
        }
    }
}