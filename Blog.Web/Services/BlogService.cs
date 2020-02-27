using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Blog.Web.Abstractions;
using Blog.Web.Config;
using Blog.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WaterHub.Core;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

namespace Blog.Web.Services
{
    public class BlogService : IBlogService
    {
        
        private readonly IBlogRepository _repository;
        private readonly IImageProcessService _imageProcessService;
        private readonly string _uploadImageRootPath;
        private readonly ISettings _settings;

        public BlogService(IBlogRepository repository, ISettings settings, IWebHostEnvironment env,
            IImageProcessService imageProcessService)
        {
            _settings = settings;
            _repository = repository;
            _imageProcessService = imageProcessService;

            _uploadImageRootPath = Path.Combine(env.WebRootPath, _settings.UploadImageRootPath);
            if (!Directory.Exists(_uploadImageRootPath))
                Directory.CreateDirectory(_uploadImageRootPath);

            var thumbRootPath = Path.Combine(env.WebRootPath, _settings.UploadImageRootPath, Constants.Thumbs);
            if (!Directory.Exists(thumbRootPath))
                Directory.CreateDirectory(thumbRootPath);
        }

        public async Task<ProcessResult<Post>> SaveUploadImagesAsync(Guid postKey, ICollection<IFormFile> files)
        {
            var processResult = _repository.GetPostByKey(postKey);
            if (!processResult.IsOk)
            {
                return processResult;
            }

            var post = processResult.Data;
            var postImages = new List<PostImage>();
            foreach (var file in files)
            {
                var postImage = new PostImage
                {
                    Extension = Path.GetExtension(file.FileName)
                }.EnsureValidKey();

                var filePath = Path.Combine(_uploadImageRootPath, postImage.FilePath);
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }
                var thumbPath = Path.Combine(_uploadImageRootPath, postImage.ThumbPath);
                await _imageProcessService.ResizeByHeightAsync(filePath, thumbPath, _settings.ThumbHeight);

                postImages.Add(postImage);
            }

            post.WithPostImages(postImages);
            return processResult;
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