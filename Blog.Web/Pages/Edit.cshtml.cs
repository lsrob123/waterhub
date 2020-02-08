using Blog.Web.Abstractions;
using Blog.Web.Config;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;

namespace Blog.Web
{
    public class EditModel : BlodPageModelBase<EditModel>
    {
        public EditModel(ILogger<EditModel> logger, IAuthService authService, ITextMapService textMapService)
          : base(logger, authService, textMapService)
        {
        }

        public override string PageName => PageDefinitions.Edit.PageName;

        public override string PageTitle => PageDefinitions.Edit.PageTitle;

        public void OnGet()
        {
        }
    }
}