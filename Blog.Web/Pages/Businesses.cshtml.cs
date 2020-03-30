using System.Threading.Tasks;
using Blog.Web.Abstractions;
using Blog.Web.Config;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;

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

        [TempData]
        public string LastSubmittedFormJson { get; set; }

        public void OnGet()
        {
            VasayoForm = LastSubmittedFormJson ?? new VasayoForm();
        }

        public async Task<IActionResult> OnPostVasayoFormAsync()
        {
            var result = await _smtpService.SendMessagesAsync
                (VasayoForm.Email, _settings.VasayoEmailAccount, "Vasayo Form", VasayoForm.MoreInfo);

            if (result.HasErrors)
            {
                VasayoForm.ErrorMessage = result.ErrorMessage;
            }

            if (result.IsOk)
            {
                VasayoForm.MoreInfo = null;
                VasayoForm.HadSuccessfulSend = true;
            }

            LastSubmittedFormJson = VasayoForm;

            return RedirectToPage();
        }
    }
}