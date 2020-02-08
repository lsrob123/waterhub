using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;

namespace Blog.Web.Abstractions
{
    public abstract class BlodPageModelBase<T> : BlodPageModelBase
    where T : BlodPageModelBase
    {
        protected readonly ILogger<T> Logger;

        public BlodPageModelBase(ILogger<T> logger, IAuthService authService,
            ITextMapService textMapServic)
            : base(authService, textMapServic)
        {
            Logger = logger;
        }
    }

    public abstract class BlodPageModelBase : PageModel
    {
        protected readonly IAuthService AuthService;
        protected readonly ITextMapService T;

        protected BlodPageModelBase(IAuthService authService, ITextMapService textMapService)
        {
            AuthService = authService;
            T = textMapService;
        }

        public bool IsLoggedIn => AuthService.IsLoggedIn();
        public abstract string PageName { get; }
        public abstract string PageTitle { get; }
        public string UserName => AuthService.GetUserIdentityName();
    }
}