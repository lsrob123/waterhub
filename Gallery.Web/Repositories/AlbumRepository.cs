using System;
using System.Collections.Generic;
using System.Linq;
using Gallery.Web.Abstractions;
using Gallery.Web.Models;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace Gallery.Web.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ILogger<AlbumRepository> _logger;
        private readonly ISettings _settings;

        public AlbumRepository(ISettings settings, ILogger<AlbumRepository> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public void DeleteAlbum(string albumName)
        {
            try
            {
                using var store = new AlbumDataStore(_settings);
                var existing = GetAlbumByName(albumName);
                store.Albums.Delete(existing.Key);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }

        public Album GetAlbumByName(string name)
        {
            try
            {
                name = name.ToLower();
                using var store = new AlbumDataStore(_settings);
                var album = store.Albums
                    .FindOne(x => x.Name.ToLower() == name.ToLower());
                return album;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }

        public ICollection<Album> ListAlbums()
        {
            try
            {
                using var store = new AlbumDataStore(_settings);
                var albums = store.Albums.Find(Query.All(Query.Descending), limit: _settings.AlbumCountMax).ToList();
                //var albums = store.Albums.FindAll().OrderByDescending(x=>x.TimeUpdated).ToList();
                return albums;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new Album[] { };
            }
        }

        public ICollection<Album> ListAlbumsByKeyword(string keyword)
        {
            try
            {
                keyword = keyword.ToLower();
                using var store = new AlbumDataStore(_settings);
                var albums = store.Albums
                    .Find(x => x.Name.ToLower().Contains(keyword) || x.Description.ToLower().Contains(keyword))
                    .OrderBy(x => x.Name)
                    .ToList();
                return albums;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return new Album[] { };
            }
        }

        public void UpdateAlbum(Album album)
        {
            try
            {
                using var store = new AlbumDataStore(_settings);
                store.Albums.Delete(album.Key);
                store.Albums.Insert(album);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }
        }
    }
}