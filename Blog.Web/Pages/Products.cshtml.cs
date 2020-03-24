using Blog.Web.Abstractions;
using Blog.Web.Config;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;

namespace Blog.Web.Pages
{
    public class ProductsModel : BlodPageModelBase<ProductsModel>
    {
        public ProductsModel(ILogger<ProductsModel> logger, IAuthService authService, ITextMapService textMapService)
            : base(logger, authService, textMapService)
        {
        }

        public override string PageName => PageDefinitions.Products.PageName;

        public override string PageTitle => PageDefinitions.Products.PageTitle;

        public void OnGet()
        {

        }
    }
}
