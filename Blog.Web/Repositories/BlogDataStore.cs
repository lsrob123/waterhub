using Blog.Web.Abstractions;
using WaterHub.Core.Persistence;

namespace Blog.Web.Repositories
{
    public class BlogDataStore : LiteDbStoreBase
    {
        public BlogDataStore(ISettings settings) : base(settings)
        {
            //BsonMapper.Global.Entity<Album>().Id(x => x.Key);

            //Albums = Database.GetCollection<Album>(nameof(Album));

            //Albums.EnsureIndex(x => x.Name, true);
            //Albums.EnsureIndex(x => x.TimeCreated, false);
        }

        //public ILiteCollection<Album> Albums { get; }
    }
}