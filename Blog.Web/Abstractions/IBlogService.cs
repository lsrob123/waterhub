using System;
using System.Collections.Generic;
using Blog.Web.Models;
using WaterHub.Core.Models;

namespace Blog.Web.Abstractions
{
    public interface IBlogService
    {
        ListLatestPostsResponse ListLatestPosts(int? postCount = null);
        ProcessResult UpsertPosts(Post post);
        ProcessResult DeletePost(Guid postKey);
        ICollection<Post> ListPostsByTags(IEnumerable<string> keywords);
        GetPostResponse GetPostByKey(Guid postKey);
    }
}
