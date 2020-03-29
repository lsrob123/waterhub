using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

namespace WaterHub.Core.Services
{
    public class SiteMapService : ISiteMapService
    {
        private readonly XNamespace NS = "http://www.sitemaps.org/schemas/sitemap/0.9";

        private readonly List<SiteMapUrl> _urls;

        public SiteMapService()
        {
            _urls = new List<SiteMapUrl>();
        }

        public void AddUrl(string url, DateTime? modified = null, SiteMapChangeFrequency? SiteMapChangeFrequency = null,
            double? priority = null, string baseUrl = null)
        {
            Add(new SiteMapUrl()
            {
                Url = url,
                Modified = modified,
                ChangeFrequency = SiteMapChangeFrequency,
                Priority = priority,
            }, baseUrl);
        }

        public void AddRange(IEnumerable<SiteMapUrl> siteMapUrls, string baseUrl = null)
        {
            foreach (var siteMapUrl in siteMapUrls)
            {
                Add(siteMapUrl, baseUrl);
            }
        }

        public void Add(SiteMapUrl siteMapUrl, string baseUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(baseUrl))
                siteMapUrl.Url = $"{baseUrl.TrimEnd('/')}/{siteMapUrl.Url}";

            _urls.Add(siteMapUrl);
        }

        public override string ToString()
        {
            var sitemap = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(NS + "urlset", _urls.Select(x => CreateItemElement(x))));

            return sitemap.ToString();
        }

        private XElement CreateItemElement(SiteMapUrl url)
        {
            XElement itemElement = new XElement(NS + "url", new XElement(NS + "loc", url.Url.ToLower()));

            if (url.Modified.HasValue)
            {
                itemElement.Add(new XElement(NS + "lastmod", url.Modified.Value.ToString("yyyy-MM-ddTHH:mm:ss.f") + "+00:00"));
            }

            if (url.ChangeFrequency.HasValue)
            {
                itemElement.Add(new XElement(NS + "changefreq", url.ChangeFrequency.Value.ToString().ToLower()));
            }

            if (url.Priority.HasValue)
            {
                itemElement.Add(new XElement(NS + "priority", url.Priority.Value.ToString("N1")));
            }

            return itemElement;
        }

        public void Clear()
        {
            _urls.Clear();
        }
    }
}
