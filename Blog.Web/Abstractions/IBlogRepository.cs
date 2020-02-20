using Blog.Web.Models;
using System;
using System.Collections.Generic;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

namespace Blog.Web.Abstractions
{
    public interface IBlogRepository : IUserQuery
    {
        ProcessResult<Post> DeletePost(Guid postKey);

        ProcessResult<Post> GetPostByKey(Guid postKey);

        ProcessResult<Post> GetPostByUrlFriendlyTitle(string urlFriendlyTitle);

        UserModel GetUserByUsername(string username);

        ICollection<string> ListAllTags();

        ICollection<PostInfoEntry> ListLatestPostInfoEntries(int? postCount = null, bool includeUnpublishedPosts = false);

        ICollection<Post> ListLatestPosts(int? postCount = null, bool includeUnpublishedPosts = false);

        ICollection<PostInfoEntry> ListPostInfoEntriesByKeywordsInTitle(IEnumerable<string> keywords, bool includeUnpublishedPosts = false);

        ICollection<PostInfoEntry> ListPostInfoEntriesByTags(IEnumerable<string> keywords, bool includeUnpublishedPosts = false);

        ICollection<Post> ListStickyPosts(int? postCount = null, bool includeUnpublishedPosts = false);

        ProcessResult<Post> UpsertPost(Post post);
    }
}