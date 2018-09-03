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
    [Area("FinalFantasyFighter")]
    public class FinalFantasyFighterController : Controller
    {
        [Route("ffxi-fighter")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
