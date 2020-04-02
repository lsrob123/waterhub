using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Web.Abstractions;
using Blog.Web.Config;
using Microsoft.AspNetCore.Mvc;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

namespace Blog.Web.Controllers
{
    public class SiteMapController : ControllerBase
    {
        private readonly ISiteMapService _siteMapService;
        private readonly IBlogService _blogService;
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

        public SiteMapController(ISiteMapService siteMapService, IBlogService blogService)
        {
            _siteMapService = siteMapService;
            _blogService = blogService;
        }

        [Route("sitemap")]
        public async Task<ActionResult> GetSitemapAsync()
        {
            string baseUrl = "https://health-findings.com/";
            _siteMapService.Clear();

            _siteMapService.AddRange(_fixedPageUrls, baseUrl);

            var posts = _blogService.ListLatestPostInfoEntries(300);
            _siteMapService.AddRange(posts.Select(x => new SiteMapUrl
            {
                Url = $"posts/{x.UrlFriendlyTitle}"
            }), baseUrl);

            string xml = await Task.FromResult(_siteMapService.ToString());
            return Content(xml, "text/xml");
        }
    }
}
