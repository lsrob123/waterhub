using Blog.Web.Abstractions;
using Blog.Web.Models;
using Microsoft.Extensions.Logging;
using System;
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
            if (username.Equals(UserModel.Admin, StringComparison.InvariantCultureIgnoreCase))
                return new UserModel
                {
                    MobilePhone = UserModel.Admin,
                    HashedPassword = _settings.AdminHashedPassword,
                    IsAdmin = true
                };

            try
            {
                username = username.ToLower();
                using var store = new BlogDataStore(_settings);
                var album = store.Users
                    .FindOne(x => x.MobilePhone.ToLower() == username.ToLower());
                return album;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }
        }
    }
}