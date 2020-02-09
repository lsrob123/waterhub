namespace WaterHub.Core.Abstractions
{
    public interface IUserModelBase
    {
        string Username { get; set; }
        string HashedPassword { get; set; }
        bool IsAdmin { get; set; }
        string PlainTextPassword { get; set; }
        string MobilePhone { get; set; }
        string Email { get; set; }
    }
}