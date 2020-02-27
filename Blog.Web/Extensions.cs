using Blog.Web.Abstractions;
using Blog.Web.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WaterHub.Core.Abstractions;

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

        public static ICollection<PostInfoEntry> SetFrontEndTexts(this ITextMapService t, ICollection<PostInfoEntry> entries, string context, string textClickToReadFullArticle, string textReadFullArticle, string textOpenArticleInNewWindow)
        {
            if (entries != null)
                foreach (var entry in entries)
                {
                    entry.TextClickToReadFullArticle = t.GetMap(textClickToReadFullArticle, context);
                    entry.TextOpenArticleInNewWindow = t.GetMap(textOpenArticleInNewWindow, context);
                    entry.TextReadFullArticle = t.GetMap(textOpenArticleInNewWindow, context);
                }

            return entries;
        }

        public static IActionResult ImageFrom(this ControllerBase controller, string filePath)
        {
            var image = File.OpenRead(filePath);
            var extension = Path.GetExtension(filePath)?.Trim().ToLower();
            if (extension == null)
                return controller.NotFound();

            var contentType = extension switch
            {
                ".gif" => "image/gif",
                ".png" => "image/png",
                _ => "image/jpg",
            };
            return controller.File(image, contentType);
        }
    }
}