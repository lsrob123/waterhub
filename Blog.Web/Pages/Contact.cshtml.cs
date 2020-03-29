using System.Threading.Tasks;
using Blog.Web.Abstractions;
using Blog.Web.Config;
using Blog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public override string PageName => PageDefinitions.Businesses.PageName;

        public override string PageTitle => PageDefinitions.Businesses.PageTitle;

        [BindProperty]
        public ContactForm ContactForm { get; set; }

        [TempData]
        public string LastSubmittedFormJson { get; set; }

        public void OnGet()
        {
            ContactForm = LastSubmittedFormJson ?? new ContactForm();
        }

        public async Task<IActionResult> OnPostContactFormAsync()
        {
            var result = await _smtpService.SendMessagesAsync
                (ContactForm.Email, _settings.SupportEmailAddress, ContactForm.Subject, ContactForm.Body);

            if (result.HasErrors)
                ContactForm.ErrorMessage = result.ErrorMessage;

            if (result.IsOk)
            {
                ContactForm.Subject = null;
                ContactForm.Body = null;
            }

            LastSubmittedFormJson = ContactForm;

            return RedirectToPage();
        }
    }
}
