using Blog.Web.Abstractions;
using Blog.Web.Models;
using LiteDB;
using WaterHub.Core.Persistence;

namespace Blog.Web.Repositories
{
    public class BlogDataStore : LiteDbStoreBase
    {
        public BlogDataStore(ISettings settings) : base(settings)
        {
            BsonMapper.Global.Entity<UserModel>().Id(x => x.Key);
            Users = Database.GetCollection<UserModel>(nameof(Users));
            Users.EnsureIndex(x => x.MobilePhone, true);

            BsonMapper.Global.Entity<Post>().Id(x => x.Key);
            Posts = Database.GetCollection<Post>(nameof(Posts));
            Posts.EnsureIndex(x => x.Title);
            Posts.EnsureIndex(x => x.UrlFriendlyTitle);
            Posts.EnsureIndex(x => x.TimeCreated);

            BsonMapper.Global.Entity<Tag>().Id(x => x.Key);
            Tags = Database.GetCollection<Tag>(nameof(Tags));
            Tags.EnsureIndex(x => x.Text);
            Tags.EnsureIndex(x => x.PostKey);

            BsonMapper.Global.Entity<PostInfoEntry>().Id(x => x.Key);
            PostInfoEntries = Database.GetCollection<PostInfoEntry>(nameof(PostInfoEntries));
            PostInfoEntries.EnsureIndex(x => x.Title);
            PostInfoEntries.EnsureIndex(x => x.UrlFriendlyTitle);
            PostInfoEntries.EnsureIndex(x => x.TimeCreated);
        }

        public ILiteCollection<Post> Posts { get; }
        public ILiteCollection<Tag> Tags { get; }
        public ILiteCollection<UserModel> Users { get; }
        public ILiteCollection<PostInfoEntry> PostInfoEntries { get; }
    }
}