using System;

namespace WaterHub.Core.Abstractions
{
    public interface ISerializationService
    {
        object Deserialize(string json, Type returnType);
        T Deserialize<T>(string json);
        string Serialize(object input);
    }
}