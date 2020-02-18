using System.Collections.Generic;
using System.Linq;
using Blog.Web.Models;

namespace Blog.Web
{
    public static class Extensions
    {
        public static bool TitleContains(this Post x, ICollection<string> keywordList)
        {
            var title = x?.Title?.Trim()?.ToLower();
            if (string.IsNullOrEmpty(title))
                return false;

            return keywordList.Any(y => x.Title.Contains(y));
        }
    }
}
