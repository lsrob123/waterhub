using System;

namespace WaterHub.Core.Models
{
    public abstract class EntityBase
    {
        public virtual Guid Key { get; set; }
        public DateTimeOffset TimeCreated { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset TimeUpdated { get; set; } = DateTimeOffset.UtcNow;
    }
}
