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

namespace soverance.com.FinalFantasyFighter.Controllers
{
    [Authorize]
    [Area("FinalFantasyFighter")]
    public class FinalFantasyFighterController : Controller
    {
        [AllowAnonymous]
        [Route("ffxi-fighter")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("ffxi-fighter/documentation")]
        public IActionResult Documentation()
        {
            return View();
        }
    }
}
