using Blog.Web.Abstractions;
using Blog.Web.Config;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;

namespace Blog.Web
{
    public class BusinessesModel : BlodPageModelBase<BusinessesModel>
    {
        public BusinessesModel(ILogger<BusinessesModel> logger, IAuthService authService, ITextMapService textMapService)
        : base(logger, authService, textMapService)
        {
        }

        public override string PageName => PageDefinitions.Businesses.PageName;

        public override string PageTitle => PageDefinitions.Businesses.PageTitle;

        public void OnGet()
        {

        }
    }
}