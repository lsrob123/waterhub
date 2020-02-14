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
        ICollection<Post> ListLatestPosts(int? postCount = null);
        ICollection<Post> ListStickyPosts(int? postCount = null);
        Post GetPostByKey(Guid postKey);
        ProcessResult UpsertPosts(Post post);
        ProcessResult DeletePost(Guid postKey);
        ICollection<Post> ListPostsByTags(IEnumerable<string> keywords);
    }
}