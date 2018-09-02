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
        private readonly IOptions<SecretConfig> SovSecretConfig;

        public HomeController(DatabaseContext _SovDatabaseContext, IOptions<SecretConfig> _SovSecretConfig)
        {
            // construct the configuration objects for this controller
            SovDatabaseContext = _SovDatabaseContext;
            SovSecretConfig = _SovSecretConfig;
        }

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

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("contact")]
        public IActionResult Contact()
        {
            ViewBag.AzureMapsKey = SovSecretConfig.Value.AzureMapsKey;
            return View(SovSecretConfig);
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
