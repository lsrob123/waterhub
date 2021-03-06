﻿using Blog.Web.Abstractions;
using Blog.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WaterHub.Core;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

namespace Blog.Web.Services
{
    public class BlogService : IBlogService
    {
        private readonly IImageProcessService _imageProcessService;
        private readonly IBlogRepository _repository;
        private readonly ISettings _settings;
        private readonly string _uploadImageRootPath;

        public BlogService(IBlogRepository repository, ISettings settings, IWebHostEnvironment env,
            IImageProcessService imageProcessService)
        {
            _settings = settings;
            _repository = repository;
            _imageProcessService = imageProcessService;

            _uploadImageRootPath = Path.Combine(env.WebRootPath, _settings.UploadImageRootPath);
            if (!Directory.Exists(_uploadImageRootPath))
                Directory.CreateDirectory(_uploadImageRootPath);

            var thumbRootPath = Path.Combine(env.WebRootPath, _settings.UploadImageRootPath, Config.Constants.Thumbs);
            if (!Directory.Exists(thumbRootPath))
                Directory.CreateDirectory(thumbRootPath);
        }

        public ProcessResult<Post> DeletePost(Guid postKey)
        {
            return _repository.DeletePost(postKey);
        }

        public ProcessResult<Post> DeletePostImage(string urlFriendlyTitle, Guid imageKey)
        {
            var getPostResult = GetPostByUrlFriendlyTitle(urlFriendlyTitle);
            if (getPostResult.Post == null)
                return new ProcessResult<Post>().AsError(new Exception("No post found"), null, HttpStatusCode.NotFound);

            getPostResult.Post.DeletePostImage(imageKey, DeletePostImageFiles);

            var upsertPostResult = UpsertPost(getPostResult.Post);
            return upsertPostResult;
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

        public GetPostImagePathResult GetPostImagePaths(string urlFriendlyTitle, string imageName)
        {
            imageName = Path.GetFileNameWithoutExtension(imageName.Trim());
            if (!int.TryParse(imageName, out var internalId))
                return new GetPostImagePathResult()
                   .AsError(HttpStatusCode.BadRequest, "Invalid image name");

            var postResult = GetPostByUrlFriendlyTitle(urlFriendlyTitle);
            if (postResult.Post == null)
                return new GetPostImagePathResult()
                    .AsError(HttpStatusCode.NotFound, "No post found");

            var postImage = postResult.Post.Images.SingleOrDefault(x => x.InternalId == internalId);
            if (postImage == null)
                return new GetPostImagePathResult()
                    .AsError(HttpStatusCode.NotFound, "No image found in the post");

            string filePath = GetPostImagePath(postImage.FilePath);
            if (!File.Exists(filePath))
                return new GetPostImagePathResult()
                    .AsError(HttpStatusCode.NotFound, "No image file found");

            var thumbnailPath = Path.Combine(_uploadImageRootPath, postImage.ThumbPath);

            return new GetPostImagePathResult().AsOk(filePath, thumbnailPath);
        }

        public string GetPostImageVirtualUrl(Post post, PostImage postImage)
        {
            return $"/posts/{post.UrlFriendlyTitle}/images/${postImage.InternalId}{postImage.Extension}";
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

                var filePath = GetPostImagePath(postImage.FilePath);
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }
                var thumbPath = Path.Combine(_uploadImageRootPath, postImage.ThumbPath);
                await _imageProcessService.ResizeByHeightAsync(filePath, thumbPath, _settings.ThumbHeight);

                postImages.Add(postImage);
            }

            post.WithPostImages(postImages);
            processResult = _repository.UpsertPost(post);
            return processResult;
        }

        public ProcessResult<Post> UpsertPost(Post post)
        {
            var result = _repository.UpsertPost(post.BuildUrlFriendlyTitle().WithUpdateOnTimeUpdated());
            return result;
        }

        private void DeletePostImageFiles(string filePath, string thumbPath)
        {
            File.Delete(Path.Combine(_uploadImageRootPath, filePath));
            File.Delete(Path.Combine(_uploadImageRootPath, thumbPath));
        }

        private string GetPostImagePath(string filePath)
        {
            return Path.Combine(_uploadImageRootPath, filePath);
        }
    }
}