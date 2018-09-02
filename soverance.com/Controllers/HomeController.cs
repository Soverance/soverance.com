using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using soverance.com.Models;

namespace soverance.com.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext SovDatabaseContext;
        private readonly IOptions<AzureConfig> SovAzureConfig;

        public HomeController(DatabaseContext _SovDatabaseContext, IOptions<AzureConfig> _SovAzureConfig)
        {
            // construct the configuration objects for this controller
            SovDatabaseContext = _SovDatabaseContext;
            SovAzureConfig = _SovAzureConfig;
        }

        [Route("")]
        [Route("home")]
        [Route("home/index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            ViewBag.AzureMapsKey = SovAzureConfig.Value.AzureMapsKey;
            return View(SovAzureConfig);
        }

        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("faq")]
        public IActionResult FAQ()
        {
            return View();
        }

        [Route("games")]
        public IActionResult Games()
        {
            return View();
        }

        [Route("testimonials")]
        public IActionResult Testimonials()
        {
            return View();
        }

        [Route("consulting")]
        public IActionResult Consulting()
        {
            return View();
        }

        [Route("gallery")]
        public IActionResult Gallery()
        {
            return View();
        }

        [Route("error")]
        public IActionResult Error()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var error = feature?.Error;
            return View("~/Views/Shared/Error.cshtml", error);
        }
    }
}
