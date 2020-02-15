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

        ListLatestPostsResponse ListLatestPosts(int? postCount = null);

        ICollection<Post> ListPostsByTags(IEnumerable<string> keywords);

        ProcessResult<Post> UpsertPost(Post post);
    }
}