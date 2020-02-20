using Blog.Web.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Web
{
    public static class Extensions
    {
        public static bool TitleContains<TPost>(this TPost x, ICollection<string> keywordList)
            where TPost : IPostInfo
        {
            var title = x?.Title?.Trim()?.ToLower();
            if (string.IsNullOrEmpty(title))
                return false;

            return keywordList.Any(y => x.Title.Contains(y));
        }
    }
}