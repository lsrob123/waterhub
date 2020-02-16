using System.Collections.Generic;

namespace Blog.Web.Models
{
    public class GetPostResponse
    {
        public Post Post { get; set; }
        public ICollection<string> AllTags { get; set; }
    }
}
