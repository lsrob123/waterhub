using Blog.Web.Abstractions;
using Blog.Web.Config;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;

namespace Blog.Web.Pages
{
    public class IndexModel : BlodPageModelBase<IndexModel>
    {
        public IndexModel(ILogger<IndexModel> logger, IAuthService authService, ITextMapService textMapService)
            : base(logger, authService, textMapService)
        {
        }

        public override string PageName => PageDefinitions.Home.PageName;

        public override string PageTitle => PageDefinitions.Home.PageTitle;

        public void OnGet()
        {
        }
    }
}