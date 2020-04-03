using Blog.Web.Abstractions;
using Blog.Web.Config;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;

namespace Blog.Web.Pages
{
    public class ProductsModel : BlodPageModelBase<ProductsModel>
    {
        private readonly ISettings _settings;

        public ProductsModel(ILogger<ProductsModel> logger, IAuthService authService, ITextMapService textMapService,
            ISettings settings)
            : base(logger, authService, textMapService)
        {
            _settings = settings;
        }

        public override string PageName => PageDefinitions.Products.PageName;

        public override string PageTitle => PageDefinitions.Products.PageTitle;

        public bool SuspendBusinessDisplay => _settings.SuspendBusinessDisplay;

        public void OnGet()
        {

        }
    }
}
