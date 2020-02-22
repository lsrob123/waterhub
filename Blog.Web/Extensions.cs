using Blog.Web.Abstractions;
using Blog.Web.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Web
{
    public static class Extensions
    {
        public static IDictionary<Guid, PostInfoEntry> AddRange(this IDictionary<Guid, PostInfoEntry> entries, IEnumerable<PostInfoEntry> result)
        {
            foreach (var entry in result)
            {
                if (!entries.ContainsKey(entry.Key))
                    entries.Add(entry.Key, entry);
            }
            return entries;
        }

        public static bool TitleContains<TPost>(this TPost x, ICollection<string> keywordList)
                    where TPost : IPostInfo
        {
            var title = x?.Title?.Trim()?.ToLower();
            if (string.IsNullOrEmpty(title))
                return false;

            return keywordList.Any(y => x.Title.Contains(y));
        }

        public static BsonExpression ContainedBy(this List<string> keywordList, string field)
        {
            var firstKeyword = keywordList.First();
            var expression = keywordList.Count == 1
                ? Query.Contains(field, firstKeyword)
                : Query.And(keywordList.Select(x => Query.Contains(field, x)).ToArray());

            return expression;
        }

        public static BsonExpression IncludeUnpublishedPosts(this BsonExpression expression, bool includeUnpublishedPosts)
        {
            if (!includeUnpublishedPosts)
                expression = Query.And(expression, Query.EQ("IsPublished", true));
            return expression;
        }
    }
}