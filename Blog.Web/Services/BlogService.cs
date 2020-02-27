using System;
using System.Collections.Generic;
using System.IO;
using Blog.Web.Abstractions;
using Blog.Web.Models;
using Microsoft.AspNetCore.Hosting;
using WaterHub.Core;
using WaterHub.Core.Models;

namespace Blog.Web.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _repository;
        private readonly string _uploadImageRootPath;

        public BlogService(IBlogRepository repository, ISettings settings, IWebHostEnvironment env)
        {
            _repository = repository;
            _uploadImageRootPath = Path.Combine(env.WebRootPath, settings.UploadImageRootPath);
        }

        private string CreateImageFilePath(string folderName, string fileName)
        {
            var path= Path.Combine(_uploadImageRootPath, folderName, fileName);
            if (File.Exists(path))
                return null;
            return path;
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

        public ICollection<PostInfoEntry> ListLatestPostInfoEntries(int? postCount = null, bool includeUnpublishedPosts = false)
        {
            return _repository.ListLatestPostInfoEntries(postCount, includeUnpublishedPosts);
        }

        public ICollection<Post> ListLatestPosts(int? postCount = null, bool includeUnpublishedPosts = false)
        {
            return _repository.ListLatestPosts(postCount, includeUnpublishedPosts);
        }

        public ICollection<PostInfoEntry> ListPostInfoEntriesByKeywords(IEnumerable<string> keywords, bool includeUnpublishedPosts = false)
        {
            var entries = new Dictionary<Guid, PostInfoEntry>()
                .AddRange(_repository.ListPostInfoEntriesByKeywordsInTitle(keywords, includeUnpublishedPosts))
                .AddRange(_repository.ListPostInfoEntriesByTags(keywords, includeUnpublishedPosts));
            return entries.Values;
        }

        public ICollection<PostInfoEntry> ListPostInfoEntriesByKeywordsInTitle(IEnumerable<string> keywords,
            bool includeUnpublishedPosts = false)
        {
            return _repository.ListPostInfoEntriesByKeywordsInTitle(keywords, includeUnpublishedPosts);
        }

        public ICollection<PostInfoEntry> ListPostInfoEntriesByTags(IEnumerable<string> keywords, bool includeUnpublishedPosts = false)
        {
            return _repository.ListPostInfoEntriesByTags(keywords, includeUnpublishedPosts);
        }

        public ICollection<Post> ListStickyPosts(int? postCount = null, bool includeUnpublishedPosts = false)
        {
            return _repository.ListStickyPosts(postCount, includeUnpublishedPosts);
        }

        public ProcessResult<Post> UpsertPost(Post post)
        {
            var result = _repository.UpsertPost(post.BuildUrlFriendlyTitle().WithUpdateOnTimeUpdated());
            return result;
        }

    }
}