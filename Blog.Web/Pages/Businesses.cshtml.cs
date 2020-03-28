using System.Threading.Tasks;
using Blog.Web.Abstractions;
using Blog.Web.Config;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;
using WaterHub.Core.Models;

namespace Blog.Web
{
    public class BusinessesModel : BlodPageModelBase<BusinessesModel>
    {
        private readonly ISettings _settings;
        private readonly ISmtpService _smtpService;

        public BusinessesModel(ILogger<BusinessesModel> logger, IAuthService authService, ITextMapService textMapService,
            ISettings settings, ISmtpService smtpService)
        : base(logger, authService, textMapService)
        {
            _settings = settings;
            _smtpService = smtpService;
        }

        public override string PageName => PageDefinitions.Businesses.PageName;

        public override string PageTitle => PageDefinitions.Businesses.PageTitle;

        [BindProperty]
        public VasayoForm VasayoForm { get; set; }

        public bool HasVasayoFormErrorMessage => !string.IsNullOrWhiteSpace(VasayoFormErrorMessage);

        [TempData]
        public string VasayoFormErrorMessage { get; set; }

        public void OnGet()
        {
            VasayoForm = new VasayoForm();
        }

        public async Task<IActionResult> OnPostVasayoFormAsync()
        {
            var result = await _smtpService.SendMessagesAsync
                (VasayoForm.Email, _settings.VasayoEmailAddress, "Vasayo Form", VasayoForm.MoreInfo);

            if (result.HasErrors)
                VasayoFormErrorMessage = result.ErrorMessage;

            return RedirectToPage();
        }
    }
}