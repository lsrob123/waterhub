using Blog.Web.Models;
using WaterHub.Core.Abstractions;

namespace Blog.Web.Abstractions
{
    public interface IBlogRepository : IUserQuery
    {
        UserModel GetUserByUsername(string username);
    }
}