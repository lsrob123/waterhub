using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Web.Abstractions;
using Blog.Web.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WaterHub.Core.Abstractions;

namespace Blog.Web
{
    public class SponsorsModel : BlodPageModelBase<SponsorsModel>
    {
        public SponsorsModel(ILogger<SponsorsModel> logger, IAuthService authService, ITextMapService textMapService)
        : base(logger, authService, textMapService)
        {
        }

        public override string PageName => PageDefinitions.Sponsors.PageName;

        public override string PageTitle => PageDefinitions.Sponsors.PageTitle;

        public void OnGet()
        {

        }
    }
}