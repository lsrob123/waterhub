using System;
using System.Diagnostics.CodeAnalysis;

namespace WaterHub.Core.Models
{
    public class TextMapKey : IComparable<TextMapKey>, IEquatable<TextMapKey>
    {
        public const string UnspecifiedContext = nameof(UnspecifiedContext);

        public TextMapKey()
        {
        }

        public TextMapKey(string key, string context)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            Key = key.Trim().ToLower();
            Context = string.IsNullOrWhiteSpace(context)
                ? UnspecifiedContext
                : context.Trim().ToLower();
        }

        public TextMapKey(TextMapEntry entry) : this(entry.Key, entry.Context)
        {
        }

        public string Key { get; set; }
        public string Context { get; set; } = UnspecifiedContext;

        public int CompareTo([AllowNull] TextMapKey other)
        {
            if (other is null) return 1;

            var context = Context.CompareTo(other.Context);
            if (context != 0)
                return context;

            return Key.CompareTo(other.Key);
        }

        public bool Equals([AllowNull] TextMapKey other)
        {
            return CompareTo(other) == 1;
        }
    }
}
