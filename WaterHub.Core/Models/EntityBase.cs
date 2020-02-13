using System;

namespace WaterHub.Core.Models
{
    public abstract class EntityBase
    {
        public Guid Key { get; protected set; }
        public DateTimeOffset TimeCreated { get; protected set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset TimeUpdated { get; protected set; } = DateTimeOffset.UtcNow;

        public void SetKey(Guid key)
        {
            Key = key;
        }
    }
}
