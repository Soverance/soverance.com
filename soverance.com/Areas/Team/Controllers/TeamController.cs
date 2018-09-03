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
using soverance.com.Models;

namespace soverance.com.Team.Controllers
{
    [Authorize]
    [Area("Team")]
    public class TeamController : Controller
    {
        [AllowAnonymous]
        [Route("team")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("team/scott-mccutchen")]
        public IActionResult ScottMcCutchen()
        {
            return View();
        }
    }
}
