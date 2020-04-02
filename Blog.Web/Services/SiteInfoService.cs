using Blog.Web.Abstractions;
using WaterHub.Core.Abstractions;

namespace Blog.Web.Services
{
    public class SiteInfoService : ISiteInfoService
    {
        private readonly ISettings _settings;
        private readonly ITextMapService _textMapService;

        public SiteInfoService(ISettings settings, ITextMapService textMapService)
        {
            _settings = settings;
            _textMapService = textMapService;
        }

        public string GetFullPageTitleForLayout(string pageTitle)
        {
            return $"{_textMapService.GetMap(_settings.SiteName)} - {pageTitle}";
        }
    }
}
