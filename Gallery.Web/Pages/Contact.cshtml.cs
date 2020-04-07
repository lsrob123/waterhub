using Gallery.Web.Abstractions;
using Gallery.Web.Config;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;

namespace Gallery.Web.Pages
{
    public class ContactModel : GalleryPageModelBase<ContactModel>
    {
        public ContactModel(ILogger<ContactModel> logger, IAuthService authService, ITextMapService textMapServic) 
            : base(logger, authService, textMapServic)
        {
        }

        public override string PageName => PageDefinitions.Contact.PageName;

        public override string PageTitle => PageDefinitions.Contact.PageTitle;

        public void OnGet()
        {

        }
    }
}