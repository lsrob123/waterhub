using System;
using System.Collections.Generic;
using WaterHub.Core.Models;

namespace WaterHub.Core.Abstractions
{
    public interface ISiteMapService
    {
        void AddUrl(string url, DateTime? modified = null, SiteMapChangeFrequency? SiteMapChangeFrequency = null, double? priority = null,
            string baseUrl = null);
        string ToString();
        void Clear();
        void Add(SiteMapUrl siteMapUrl, string baseUrl = null);
        void AddRange(IEnumerable<SiteMapUrl> siteMapUrls, string baseUrl = null);
    }
}