using System;

namespace WaterHub.Core.Models
{
    public abstract class EntityBase
    {
        public Guid Key { get; protected set; }

        public void SetKey(Guid key)
        {
            Key = key;
        }
    }
}
