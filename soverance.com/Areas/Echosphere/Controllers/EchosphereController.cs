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

namespace soverance.com.Echopshere.Controllers
{
    [Area("Echosphere")]
    public class EchosphereController : Controller
    {
        private readonly IOptions<SecretConfig> SovSecretConfig;

        public EchosphereController(DatabaseContext _SovDatabaseContext, IOptions<SecretConfig> _SovSecretConfig)
        {
            // construct the configuration objects for this controller
            SovSecretConfig = _SovSecretConfig;
        }

        [Route("echosphere")]
        public IActionResult Index()
        {
            string GoogleApiKey = SovSecretConfig.Value.GoogleApiKey;
            List<YouTubeData> EchosphereList = YouTubeModel.GetVideos(GoogleApiKey, false, "PLXZQqd9R-mFEHtjtYdgI0OtAcIn1Ta_4p");
            ViewBag.EchospherePlaylist = EchosphereList.Take(5);
            ViewBag.ShowcaseVideo = EchosphereList[0];

            return View();
        }
    }
}
