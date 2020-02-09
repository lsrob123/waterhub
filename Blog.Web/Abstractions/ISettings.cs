using WaterHub.Core.Abstractions;

namespace Blog.Web.Abstractions
{
    public interface ISettings :IHasTextMapFilePath, IHasLiteDbDatabaseName, IHasSerilogSettings
    {
        string AdminHashedPassword { get; }
    }
}