using WaterHub.Core.Models;

namespace WaterHub.Core.Abstractions
{
    public interface IUserQuery
    {
        UserModelBase GetUser(string username);
    }
}
