using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using LiteDB;
using WaterHub.Core.Persistence;

namespace Gallery.Web.Repositories
{
    public class AlbumDataStore : LiteDbStoreBase
    {
        public AlbumDataStore(ISettings settings) : base(settings)
        {
            BsonMapper.Global.Entity<Album>().Id(x => x.Key);

            Albums = Database.GetCollection<Album>(nameof(Album));
            
            Albums.EnsureIndex(x => x.Name, true);
            Albums.EnsureIndex(x => x.TimeCreated, false);
        }

        public ILiteCollection<Album> Albums { get; }
    }
}
