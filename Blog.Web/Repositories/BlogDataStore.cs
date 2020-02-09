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
        }

        public ILiteCollection<UserModel> Users { get; }
    }
}