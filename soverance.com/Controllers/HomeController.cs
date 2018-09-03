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
using Microsoft.AspNetCore.Authorization;
using soverance.com.Models;

namespace soverance.com.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DatabaseContext SovDatabaseContext;
        private readonly IOptions<SecretConfig> SovSecretConfig;

        public HomeController(DatabaseContext _SovDatabaseContext, IOptions<SecretConfig> _SovSecretConfig)
        {
            // construct the configuration objects for this controller
            SovDatabaseContext = _SovDatabaseContext;
            SovSecretConfig = _SovSecretConfig;
        }

        [AllowAnonymous]
        [Route("")]
        [Route("home")]
        [Route("home/index")]
        public IActionResult Index()
        {
            string GoogleApiKey = SovSecretConfig.Value.GoogleApiKey;
            List<YouTubeData> VideoList = YouTubeModel.GetVideos(GoogleApiKey, true);
            ViewBag.LatestVideos = VideoList.Take(4);
            ViewBag.TotalChannelViews = YouTubeModel.GetChannelTotalViews(GoogleApiKey);
            ViewBag.TotalSubscribers = YouTubeModel.GetChannelSubscriberCount(GoogleApiKey);            
            List<YouTubeData> TutorialList = YouTubeModel.GetVideos(GoogleApiKey, false, "PLXZQqd9R-mFHH3cBvJIffMUUWQjl9h1Kh");
            ViewBag.TutorialPlaylist = TutorialList.Take(4);
            ViewBag.LatestTutorial = TutorialList[0];
            return View(VideoList);
        }

        [AllowAnonymous]
        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("contact")]
        public IActionResult Contact()
        {
            ViewBag.AzureMapsKey = SovSecretConfig.Value.AzureMapsKey;
            return View(SovSecretConfig);
        }

        [AllowAnonymous]
        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("faq")]
        public IActionResult FAQ()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("games")]
        public IActionResult Games()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("testimonials")]
        public IActionResult Testimonials()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("consulting")]
        public IActionResult Consulting()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("gallery")]
        public IActionResult Gallery()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("error")]
        public IActionResult Error()
        {
            ViewData["statusCode"] = HttpContext.Response.StatusCode;
            //ViewData["message"] = HttpContext.Features.Get<IExceptionHandlerFeature>().Error.Message;
            return View();
        }
    }
}
