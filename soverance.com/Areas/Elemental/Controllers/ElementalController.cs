using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using soverance.com.Models;

namespace soverance.com.Elemental.Controllers
{
    [Authorize]
    [Area("Elemental")]
    public class ElementalController : Controller
    {
        private readonly IOptions<SecretConfig> SovSecretConfig;

        public ElementalController(IOptions<SecretConfig> _SovSecretConfig)
        {
            // construct the configuration objects for this controller
            SovSecretConfig = _SovSecretConfig;
        }

        [AllowAnonymous]
        [Route("ctf-elemental")]
        public IActionResult Index()
        {
            string GoogleApiKey = SovSecretConfig.Value.GoogleApiKey;
            List<YouTubeData> ElementalList = YouTubeModel.GetVideos(GoogleApiKey, false, "PLXZQqd9R-mFFvBMV2lVfe4vixRQKqrVUO");
            ViewBag.ElementalPlaylist = ElementalList.Take(5);
            ViewBag.ShowcaseVideo = ElementalList[0];

            return View();
        }
    }
}
