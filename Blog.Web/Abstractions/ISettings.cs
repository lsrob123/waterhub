using WaterHub.Core.Abstractions;

namespace Blog.Web.Abstractions
{
    public interface ISettings : IHashedPasswordQuery, IHasTextMapFilePath, IHasLiteDbDatabaseName, IHasSerilogSettings
    {
    }
}