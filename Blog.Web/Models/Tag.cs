using System;
using WaterHub.Core.Models;

namespace Blog.Web.Models
{
    public class Tag : EntityBase
    {
        public Guid PostKey { get; set; }
        public string Text { get; set; }
    }
}
