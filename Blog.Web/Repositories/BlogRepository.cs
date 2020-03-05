using Blog.Web.Abstractions;
using Blog.Web.Models;
using LiteDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using WaterHub.Core;
using WaterHub.Core.Models;

namespace Blog.Web.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private const string Title = nameof(PostInfoEntry.Title), Text = nameof(Tag.Text), Id = "_id";
        private readonly ILogger<BlogRepository> _logger;
        private readonly ISettings _settings;

        public BlogRepository(ISettings settings, ILogger<BlogRepository> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public ProcessResult<Post> DeletePost(Guid postKey)
        {
            try
            {
                using var store = new BlogDataStore(_settings);
                store.Posts.Delete(postKey);
                store.Tags.DeleteMany(x => x.PostKey == postKey);
                return new ProcessResult<Post>().AsOk();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        public ProcessResult<Post> GetPostByKey(Guid postKey)
        {
            try
            {
                using var store = new BlogDataStore(_settings);
                var post = GetPostByKey(store, postKey);
                return new ProcessResult<Post>().AsOk(post);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        public ProcessResult<Post> GetPostByUrlFriendlyTitle(string urlFriendlyTitle)
        {
            try
            {
                using var store = new BlogDataStore(_settings);
                var post = GetPostByUrlFriendlyTitle(store, urlFriendlyTitle);
                return new ProcessResult<Post>().AsOk(post);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
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

        public ICollection<string> ListAllTags()
        {
            try
            {
                using var store = new BlogDataStore(_settings);
                var tags = store.Tags.FindAll().Select(x => x.Text).Distinct().ToList();
                return tags;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new List<string>();
            }
        }

        public ICollection<PostInfoEntry> ListLatestPostInfoEntries(int? postCount = null, bool includeUnpublishedPosts = false)
        {
            try
            {
                postCount ??= _settings.LatestPostsCount;
                using var store = new BlogDataStore(_settings);
                var query = store.PostInfoEntries.Query();
                query = includeUnpublishedPosts
                    ? query
                    : query.Where(x => x.IsPublished);

                var posts = query
                    .OrderByDescending(x => x.TimeCreated)
                    .Limit(postCount.Value)
                    .ToList();
                return posts;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new List<PostInfoEntry>();
            }
        }

        public ICollection<Post> ListLatestPosts(int? postCount = null, bool includeUnpublishedPosts = false)
        {
            try
            {
                postCount ??= _settings.LatestPostsCount;
                using var store = new BlogDataStore(_settings);
                var query = store.Posts.Query();
                query = includeUnpublishedPosts
                    ? query
                    : query.Where(x => x.IsPublished);

                var posts = query
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

        public ICollection<PostInfoEntry> ListPostInfoEntriesByKeywordsInTitle(IEnumerable<string> keywords,
            bool includeUnpublishedPosts = false)
        {
            try
            {
                if (keywords == null || !keywords.Any())
                    return new List<PostInfoEntry>();

                var keywordList = keywords.ToList();

                using var store = new BlogDataStore(_settings);

                var expression = keywordList.ContainedBy(Title).IncludeUnpublishedPosts(includeUnpublishedPosts);

                var entries = store.PostInfoEntries.Find(expression,
                        limit: _settings.PostsFromSearchCount)
                    .OrderByDescending(x => x.TimeCreated)
                    .ToList();

                return entries;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new List<PostInfoEntry>();
            }
        }

        public ICollection<PostInfoEntry> ListPostInfoEntriesByTags(IEnumerable<string> keywords,
            bool includeUnpublishedPosts = false)
        {
            try
            {
                var keywordList = keywords.ToList();

                using var store = new BlogDataStore(_settings);
                var expression = keywordList.ContainedBy(Text);

                var postKeys = store.Tags.Find(expression)
                                    .Select(x => x.PostKey)
                                    .Distinct()
                                    .ToList();
                if (postKeys.Count == 0)
                    return new List<PostInfoEntry>();

                var firstKey = postKeys.First();
                expression = postKeys.Count == 1
                        ? Query.EQ(Id, firstKey)
                        : Query.In(Id, postKeys.Select(x => new BsonValue(x)));

                expression = expression.IncludeUnpublishedPosts(includeUnpublishedPosts);

                var entries = store.PostInfoEntries
                    .Find(expression, limit: 1000)
                    .OrderByDescending(x => x.TimeCreated)
                    .ToList();

                return entries;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new List<PostInfoEntry>();
            }
        }

        public ICollection<Post> ListStickyPosts(int? postCount = null, bool includeUnpublishedPosts = false)
        {
            try
            {
                postCount ??= _settings.LatestPostsCount;
                using var store = new BlogDataStore(_settings);

                var query = includeUnpublishedPosts
                    ? store.Posts.Query().Where(x => x.IsSticky)
                    : store.Posts.Query().Where(x => x.IsSticky && x.IsPublished);

                var posts = query
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

        public ProcessResult<Post> UpsertPost(Post post)
        {
            try
            {
                using var store = new BlogDataStore(_settings);
                store.Posts.DeleteMany(x => x.Key == post.Key);
                store.Posts.DeleteMany(x => x.Title == post.Title);
                store.Posts.DeleteMany(x => x.UrlFriendlyTitle == post.UrlFriendlyTitle);

                store.PostInfoEntries.DeleteMany(x => x.Key == post.Key);
                store.PostInfoEntries.DeleteMany(x => x.Title == post.Title);
                store.PostInfoEntries.DeleteMany(x => x.UrlFriendlyTitle == post.UrlFriendlyTitle);

                store.Tags.DeleteMany(x => x.PostKey == post.Key);

                post.Key = Guid.NewGuid();
                store.Tags.InsertBulk(post.AsTagEntities());
                store.PostInfoEntries.Insert(post.AsPostInfoEntry());
                store.Posts.Insert(post);

                return new ProcessResult<Post>().AsOk(data: post);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        private static Post GetPostByKey(BlogDataStore store, Guid postKey)
        {
            var post = store.Posts.FindById(postKey);
            return post;
        }

        private static Post GetPostByUrlFriendlyTitle(BlogDataStore store, string urlFriendlyTitle)
        {
            return store.Posts.FindOne(x => x.UrlFriendlyTitle == urlFriendlyTitle);
        }

        private ProcessResult<Post> InternalServerError(Exception e)
        {
            var errorCode = Guid.NewGuid();
            _logger.LogError(e, e.CodedErrorMessage(errorCode));
            return new ProcessResult<Post>().AsError(e, errorCode.ToString(), HttpStatusCode.InternalServerError);
        }
    }
}