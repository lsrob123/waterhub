using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Web.Config;
using Microsoft.AspNetCore.Mvc;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

namespace Blog.Web.Controllers
{
    public class SiteMapController : ControllerBase
    {
        private readonly ISiteMapService _siteMapService;
        private readonly List<SiteMapUrl> _fixedPageUrls = new List<SiteMapUrl>
        {
            new SiteMapUrl
            {
                Url=string.Empty
            },
            new SiteMapUrl
            {
                Url=PageDefinitions.Products.PageName
            },
            new SiteMapUrl
            {
                Url=PageDefinitions.Businesses.PageName
            },
            new SiteMapUrl
            {
                Url=PageDefinitions.Contact.PageName
            },
         };

        public SiteMapController(ISiteMapService siteMapService)
        {
            _siteMapService = siteMapService;
        }

        [Route("sitemap")]
        public async Task<ActionResult> GetSitemapAsync()
        {
            string baseUrl = "https://health-findings.com/";
            _siteMapService.Clear();

            //// get a list of published articles
            //var posts = await _blogService.PostService.GetPostsAsync();

            //// get last modified date of the home page
            //var siteMapBuilder = new SitemapBuilder();

            //// add the home page to the sitemap
            //siteMapBuilder.AddUrl(baseUrl, modified: DateTime.UtcNow, changeFrequency: ChangeFrequency.Weekly, priority: 1.0);

            //// add the blog posts to the sitemap
            //foreach (var post in posts)
            //{
            //    siteMapBuilder.AddUrl(baseUrl + post.Slug, modified: post.Published, changeFrequency: null, priority: 0.9);
            //}

            _siteMapService.AddRange(_fixedPageUrls, baseUrl);

            string xml = await Task.FromResult(_siteMapService.ToString());
            return Content(xml, "text/xml");
        }
    }
}
