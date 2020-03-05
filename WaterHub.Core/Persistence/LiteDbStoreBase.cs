using LiteDB;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using WaterHub.Core.Abstractions;

namespace WaterHub.Core.Persistence
{
    public abstract class LiteDbStoreBase : IDisposable
    {
        protected bool Disposed = false;
        protected readonly LiteDatabase Database;

        protected LiteDbStoreBase(IHasLiteDbDatabaseName settings)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", settings.LiteDbDatabaseName);
            var folderPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var connectionString = new ConnectionString { Filename = filePath };
            Database = new LiteDatabase(connectionString);
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                Database.Dispose();
            }

            Disposed = true;
        }
    }
}