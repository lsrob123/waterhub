using System;

namespace WaterHub.Core.Models
{
    public class SiteMapUrl
    {
        public string Url { get; set; }
        public DateTime? Modified { get; set; }
        public SiteMapChangeFrequency? ChangeFrequency { get; set; }
        public double? Priority { get; set; }
    }
}
