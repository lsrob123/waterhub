using Blog.Web.Models;
using System;
using System.Collections.Generic;
using WaterHub.Core.Models;

namespace Blog.Web.Abstractions
{
    public interface IBlogService
    {
        ProcessResult<Post> DeletePost(Guid postKey);

        GetPostResponse GetPostByKey(Guid postKey);

        GetPostResponse GetPostByUrlFriendlyTitle(string urlFriendlyTitle);

        ICollection<PostInfoEntry> ListPostInfoEntriesByTags(IEnumerable<string> keywords, bool includeUnpublishedPosts = false);

        ProcessResult<Post> UpsertPost(Post post);

        ICollection<string> ListAllTags();

        ICollection<PostInfoEntry> ListPostInfoEntriesByKeywordsInTitle(IEnumerable<string> keywords, bool includeUnpublishedPosts = false);

        ICollection<Post> ListLatestPosts(int? postCount = null, bool includeUnpublishedPosts = false);

        ICollection<Post> ListStickyPosts(int? postCount = null, bool includeUnpublishedPosts = false);
    }
}