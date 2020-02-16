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
        ProcessResult<Post> GetPostByKey(Guid postKey);
        ProcessResult<Post> GetPostByUrlFriendlyTitle(string urlFriendlyTitle);
        ProcessResult<Post> UpsertPost(Post post);
        ProcessResult<Post> DeletePost(Guid postKey);
        ICollection<Post> ListPostsByTags(IEnumerable<string> keywords);
        ICollection<string> ListAllTags();
    }
}