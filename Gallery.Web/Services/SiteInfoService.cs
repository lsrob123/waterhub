using Gallery.Web.Abstractions;
using WaterHub.Core.Abstractions;

namespace Gallery.Web.Services
{
    public class SiteInfoService : ISiteInfoService
    {
        private readonly ITextMapService _textMapService;
        private readonly ISettings _settings;

        public SiteInfoService(ITextMapService textMapService, ISettings settings)
        {
            _textMapService = textMapService;
            _settings = settings;
        }

        public string GetPageTitle(string pageTitle)
        {
            pageTitle = $"{_textMapService.GetMap(_settings.SiteName)} - {_textMapService.GetMap(pageTitle)}";
            return pageTitle;
        }
    }
}
