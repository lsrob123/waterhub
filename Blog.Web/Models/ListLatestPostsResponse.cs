using System.Collections.Generic;

namespace Blog.Web.Models
{
    public class ListLatestPostsResponse
    {
        public ICollection<Post> LatestPosts { get; set; }
        public ICollection<Post> StickyPosts { get; set; }
    }
}
