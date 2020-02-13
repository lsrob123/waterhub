namespace WaterHub.Core.Abstractions
{
    public interface ITextMapService
    {
        string GetMap(string key, string context = null);
    }
}