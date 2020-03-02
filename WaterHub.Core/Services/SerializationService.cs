using System;
using System.Text.Json;
using WaterHub.Core.Abstractions;

namespace WaterHub.Core.Services
{
    public class SerializationService : ISerializationService
    {
        private static readonly JsonSerializerOptions Options =
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true };

        public T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, Options);
        }

        public object Deserialize(string json, Type returnType)
        {
            return JsonSerializer.Deserialize(json, returnType, Options);
        }

        public string Serialize(object input)
        {
            return JsonSerializer.Serialize(input, Options);
        }
    }
}