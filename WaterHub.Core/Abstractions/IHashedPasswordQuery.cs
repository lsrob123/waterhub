namespace WaterHub.Core.Abstractions
{
    public interface IHashedPasswordQuery
    {
        string GetHashedPassword(string username);
    }
}
