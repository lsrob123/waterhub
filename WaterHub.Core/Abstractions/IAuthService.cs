using System.Threading.Tasks;

namespace WaterHub.Core.Abstractions
{
    public interface IAuthService
    {
        string HashPassword(string plainTextPassword);
        bool VerifyHashedPassword(string username, string plainTextPassword);
        Task<bool> SignInAsync(string username, string plainTextPassword);
        Task SignOutAsync();
        bool IsLoggedIn();
        string GetUserIdentityName();
    }
}