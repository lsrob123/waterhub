using System;
using System.Collections.Generic;
using Blog.Web.Abstractions;
using Blog.Web.Models;
using WaterHub.Core.Models;

namespace Blog.Web.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _repository;

        public BlogService(IBlogRepository repository)
        {
            _repository = repository;
        }

        public ProcessResult DeletePost(Guid postKey)
        {
            return _repository.DeletePost(postKey);
        }

        public GetPostResponse GetPostByKey(Guid postKey)
        {
            var response = new GetPostResponse
            {
                Post = _repository.GetPostByKey(postKey)
            };
            return response;
        }

        public ListLatestPostsResponse ListLatestPosts(int? postCount = null)
        {
            var response = new ListLatestPostsResponse
            {
                LatestPosts = _repository.ListLatestPosts(postCount),
                StickyPosts = _repository.ListStickyPosts(postCount)
            };
            return response;
        }

        public ICollection<Post> ListPostsByTags(IEnumerable<string> keywords)
        {
            return _repository.ListPostsByTags(keywords);
        }

        public ProcessResult UpsertPosts(Post post)
        {
            return _repository.UpsertPosts(post);
        }
    }
}
