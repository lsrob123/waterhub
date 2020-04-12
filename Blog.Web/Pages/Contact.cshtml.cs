using Blog.Web.Abstractions;
using Blog.Web.Config;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WaterHub.Core;
using WaterHub.Core.Abstractions;

namespace Blog.Web.Pages
{
    public class ContactModel : BlodPageModelBase<ContactModel>
    {
        private readonly ISettings _settings;
        private readonly ISmtpService _smtpService;

        public ContactModel(ILogger<ContactModel> logger, IAuthService authService, ITextMapService textMapService,
            ISettings settings, ISmtpService smtpService)
        : base(logger, authService, textMapService)
        {
            _settings = settings;
            _smtpService = smtpService;
        }

        [BindProperty]
        public ContactForm ContactForm { get; set; }

        [TempData]
        public string LastSubmittedFormJson { get; set; }

        public override string PageName => PageDefinitions.Businesses.PageName;

        public override string PageTitle => PageDefinitions.Businesses.PageTitle;

        public void OnGet()
        {
            ContactForm = LastSubmittedFormJson ?? new ContactForm();
        }

        public async Task<IActionResult> OnPostContactFormAsync()
        {
            if (!Request.IsChecked("IsHumanInput"))
            {
                ContactForm.ErrorMessage = "请点击确认输入";
                LastSubmittedFormJson = ContactForm;
                return RedirectToPage();
            }

            var result = await _smtpService.SendMessagesAsync
                (ContactForm.Email, _settings.SupportEmailAccount, ContactForm.Subject, ContactForm.Body);

            if (result.HasErrors)
            {
                ContactForm.ErrorMessage = result.ErrorMessage;
            }

            if (result.IsOk)
            {
                ContactForm.Subject = null;
                ContactForm.Body = null;
                ContactForm.HadSuccessfulSend = true;
            }

            LastSubmittedFormJson = ContactForm;

            return RedirectToPage();
        }
    }
}