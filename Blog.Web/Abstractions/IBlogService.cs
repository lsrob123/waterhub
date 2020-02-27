using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Web.Models;
using Microsoft.AspNetCore.Http;
using WaterHub.Core.Models;

namespace Blog.Web.Abstractions
{
    public interface IBlogService
    {
        ProcessResult<Post> DeletePost(Guid postKey);

        GetPostResponse GetPostByKey(Guid postKey);

        GetPostResponse GetPostByUrlFriendlyTitle(string urlFriendlyTitle);

        ICollection<string> ListAllTags();

        ICollection<PostInfoEntry> ListLatestPostInfoEntries(int? postCount = null, bool includeUnpublishedPosts = false);

        ICollection<Post> ListLatestPosts(int? postCount = null, bool includeUnpublishedPosts = false);

        ICollection<PostInfoEntry> ListPostInfoEntriesByKeywordsInTitle(IEnumerable<string> keywords, bool includeUnpublishedPosts = false);

        ICollection<PostInfoEntry> ListPostInfoEntriesByTags(IEnumerable<string> keywords, bool includeUnpublishedPosts = false);
        ICollection<PostInfoEntry> ListPostInfoEntriesByKeywords(IEnumerable<string> keywords, bool includeUnpublishedPosts = false);

        ICollection<Post> ListStickyPosts(int? postCount = null, bool includeUnpublishedPosts = false);

        ProcessResult<Post> UpsertPost(Post post);

        Task<ProcessResult<Post>> SaveUploadImagesAsync(Guid postKey, ICollection<IFormFile> files);
    }
}