using Blog.Web.Abstractions;
using Blog.Web.Models;
using System;
using System.Collections.Generic;
using WaterHub.Core;
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

        public ProcessResult<Post> DeletePost(Guid postKey)
        {
            return _repository.DeletePost(postKey);
        }

        public GetPostResponse GetPostByKey(Guid postKey)
        {
            var result = _repository.GetPostByKey(postKey);
            var response = new GetPostResponse
            {
                Post = result.IsOk ? result.Data : null,
                AllTags = ListAllTags()
            };
            return response;
        }

        public GetPostResponse GetPostByUrlFriendlyTitle(string urlFriendlyTitle)
        {
            var result = _repository.GetPostByUrlFriendlyTitle(urlFriendlyTitle);
            var response = new GetPostResponse
            {
                Post = result.IsOk ? result.Data : null,
                AllTags = ListAllTags()
            };
            return response;
        }

        public ICollection<string> ListAllTags()
        {
            return _repository.ListAllTags(); // TODO: Add caching
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

        public ProcessResult<Post> UpsertPost(Post post)
        {
            var result = _repository.UpsertPost(post.BuildUrlFriendlyTitle().WithUpdateOnTimeUpdated());
            return result;
        }
    }
}