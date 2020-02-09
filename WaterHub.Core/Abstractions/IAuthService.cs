using System.Threading.Tasks;
using WaterHub.Core.Models;

namespace WaterHub.Core.Abstractions
{
    public interface IAuthService
    {
        string GetUserIdentityName();

        string HashPassword(string plainTextPassword);

        bool IsAdmin();

        bool IsLoggedIn();

        Task<ProcessResult> SignInAsync(string username, string plainTextPassword);

        Task SignOutAsync();

        UserModelBase VerifyHashedPassword(string username, string plainTextPassword);
    }
}