using Blog.Web.Abstractions;
using Blog.Web.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WaterHub.Core.Models;

namespace Blog.Web.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ILogger<BlogRepository> _logger;
        private readonly ISettings _settings;

        public BlogRepository(ISettings settings, ILogger<BlogRepository> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public UserModelBase GetUser(string username)
        {
            return GetUserByUsername(username);
        }

        public UserModel GetUserByUsername(string username)
        {
            if (username.Equals(UserModelBase.Admin, StringComparison.InvariantCultureIgnoreCase))
                return new UserModel
                {
                    MobilePhone = UserModelBase.Admin,
                    HashedPassword = _settings.AdminHashedPassword,
                    IsAdmin = true
                };

            try
            {
                username = username.ToLower();
                using var store = new BlogDataStore(_settings);
                var user = store.Users
                    .FindOne(x => x.MobilePhone.ToLower() == username.ToLower());
                return user;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }

        public ICollection<Post> ListLatestPosts(int? postCount = null)
        {
            try
            {
                postCount ??= _settings.LatestPostsCount;
                using var store = new BlogDataStore(_settings);
                var posts = store.Posts.Query()
                    .OrderByDescending(x => x.TimeCreated)
                    .Limit(postCount.Value)
                    .ToList();
                return posts;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new List<Post>();
            }
        }

        public ProcessResult UpsertPosts(Post post)
        {
            try
            {
                using var store = new BlogDataStore(_settings);
                var existing = store.Posts.FindById(post.Key);
                if (!(existing is null))
                    store.Posts.Delete(post.Key);

                store.Tags.DeleteMany(x => x.PostKey == post.Key);
                if (!(post.Tags is null))
                    store.Tags.InsertBulk(post.Tags);

                store.Posts.Insert(post);
                return ProcessResult.OK;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return ProcessResult.InternalServerError;
            }
        }

        public ProcessResult DeletePost(Guid postKey)
        {
            try
            {
                using var store = new BlogDataStore(_settings);
                store.Posts.Delete(postKey);
                store.Tags.DeleteMany(x => x.PostKey == postKey);
                return ProcessResult.OK;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return ProcessResult.InternalServerError;
            }
        }

        public ICollection<Post> ListPostsByTags(IEnumerable<string> keywords)
        {
            try
            {
                var keywordList = keywords.Select(x => x.Trim().ToLower()).ToArray();
                using var store = new BlogDataStore(_settings);
                var postKeys = store.Tags.Find(x => keywordList.Contains(x.Text))
                    .Select(x => x.PostKey)
                    .Distinct()
                    .ToList();
                if (postKeys.Count == 0)
                    return new List<Post>();

                var posts = store.Posts.Query()
                    .Where(x => postKeys.Contains(x.Key))
                    .OrderByDescending(x => x.TimeCreated)
                    .Limit(_settings.PostsFromSearchCount)
                    .ToList();

                return posts;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new List<Post>();
            }
        }
    }
}