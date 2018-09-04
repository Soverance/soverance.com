using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using soverance.com.Models;

namespace soverance.com.EndlessReach.Controllers
{
    [Authorize]
    [Area("EndlessReach")]
    public class EndlessReachController : Controller
    {
        private readonly IOptions<SecretConfig> SovSecretConfig;

        public EndlessReachController(IOptions<SecretConfig> _SovSecretConfig)
        {
            // construct the configuration objects for this controller
            SovSecretConfig = _SovSecretConfig;
        }

        [AllowAnonymous]
        [Route("endlessreach")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("endlessreach/vr")]
        public IActionResult VR()
        {
            string GoogleApiKey = SovSecretConfig.Value.GoogleApiKey;
            List<YouTubeData> ERVRList = YouTubeModel.GetVideos(GoogleApiKey, false, "PLXZQqd9R-mFHdg05pfPQ_c3Mp-QJ6-pBz");
            ViewBag.ERVRPlaylist = ERVRList.Take(6);
            ViewBag.ShowcaseVideo = ERVRList[0];

            return View();
        }
    }
}
