using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

namespace WaterHub.Core.Services
{
    public class TextMapService : ITextMapService
    {
        private readonly SortedDictionary<TextMapKey, string> _maps
            = new SortedDictionary<TextMapKey, string>();

        public TextMapService(IHasTextMapFilePath settings) : this(LoadFromFile(settings))
        { }

        public TextMapService(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;

            var entries = JsonSerializer.Deserialize<List<TextMapEntry>>
                (json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                .OrderByDescending(x => x.Context).ThenBy(x => x.Key)
                .ToList();

            foreach (var entry in entries)
            {
                var key = new TextMapKey(entry);
                if (!_maps.ContainsKey(key))
                    _maps.Add(key, entry.Value);
            }
        }

        public string GetMap(string key, string context = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            var searchKey = new TextMapKey(key,
                string.IsNullOrWhiteSpace(context) ? TextMapKey.__UnspecifiedContext : context);

            if (_maps.TryGetValue(searchKey, out var value))
                return value;

            return key;
        }

        private static string LoadFromFile(IHasTextMapFilePath settings)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), settings.TextMapFilePath);
            if (!File.Exists(path))
                return null;

            var json = File.ReadAllText(path);
            return json;
        }
    }
}