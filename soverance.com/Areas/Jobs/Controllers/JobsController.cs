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

namespace soverance.com.Jobs.Controllers
{
    [Authorize]
    [Area("Jobs")]
    public class JobsController : Controller
    {
        [AllowAnonymous]
        [Route("jobs")]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("jobs/combat-engineer")]
        public IActionResult CombatEngineer()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("jobs/level-designer")]
        public IActionResult LevelDesigner()
        {
            return View();
        }
    }
}
