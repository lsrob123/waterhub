using System;
using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class Comment : EntityBase
    {
        public Guid PostKey { get; set; }
        public string Content { get; set; }
    }
}
