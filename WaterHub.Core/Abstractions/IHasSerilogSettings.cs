using WaterHub.Core.Models;

namespace WaterHub.Core.Abstractions
{
    public interface IHasSerilogSettings
    {
        public SerilogSettings SerilogSettings { get; }
    }
}
