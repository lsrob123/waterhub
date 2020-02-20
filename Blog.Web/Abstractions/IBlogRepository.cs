using System;
using System.Collections.Generic;
using Blog.Web.Models;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

namespace Blog.Web.Abstractions
{
    public interface IBlogRepository : IUserQuery
    {
        UserModel GetUserByUsername(string username);
        ICollection<Post> ListLatestPosts(int? postCount = null, bool includeUnpublishedPosts = false);
        ICollection<Post> ListStickyPosts(int? postCount = null, bool includeUnpublishedPosts = false);
        ProcessResult<Post> GetPostByKey(Guid postKey);
        ProcessResult<Post> GetPostByUrlFriendlyTitle(string urlFriendlyTitle);
        ProcessResult<Post> UpsertPost(Post post);
        ProcessResult<Post> DeletePost(Guid postKey);
        ICollection<PostInfoEntry> ListPostInfoEntriesByTags(IEnumerable<string> keywords, bool includeUnpublishedPosts = false);
        ICollection<string> ListAllTags();
        ICollection<PostInfoEntry> ListPostInfoEntriesByKeywordsInTitle(IEnumerable<string> keywords, bool includeUnpublishedPosts = false);
    }
}