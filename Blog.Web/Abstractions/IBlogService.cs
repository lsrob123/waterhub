using Blog.Web.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaterHub.Core.Models;

namespace Blog.Web.Abstractions
{
    public interface IBlogService
    {
        ProcessResult<Post> DeletePost(Guid postKey);

        ProcessResult<Post> DeletePostImage(string urlFriendlyTitle, Guid imageKey);

        GetPostResponse GetPostByKey(Guid postKey);

        GetPostResponse GetPostByUrlFriendlyTitle(string urlFriendlyTitle);

        GetPostImagePathResult GetPostImagePaths(string urlFriendlyTitle, string imageName);

        string GetPostImageVirtualUrl(Post post, PostImage postImage);

        ICollection<string> ListAllTags();

        ICollection<PostInfoEntry> ListLatestPostInfoEntries(int? postCount = null, bool includeUnpublishedPosts = false);

        ICollection<Post> ListLatestPosts(int? postCount = null, bool includeUnpublishedPosts = false);

        ICollection<PostInfoEntry> ListPostInfoEntriesByKeywords(IEnumerable<string> keywords, bool includeUnpublishedPosts = false);

        ICollection<PostInfoEntry> ListPostInfoEntriesByKeywordsInTitle(IEnumerable<string> keywords, bool includeUnpublishedPosts = false);

        ICollection<PostInfoEntry> ListPostInfoEntriesByTags(IEnumerable<string> keywords, bool includeUnpublishedPosts = false);

        ICollection<Post> ListStickyPosts(int? postCount = null, bool includeUnpublishedPosts = false);

        Task<ProcessResult<Post>> SaveUploadImagesAsync(Guid postKey, ICollection<IFormFile> files);

        ProcessResult<Post> UpsertPost(Post post);
    }
}