﻿using Blog.Web.Abstractions;
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