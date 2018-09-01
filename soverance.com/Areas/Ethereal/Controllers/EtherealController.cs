using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using soverance.com.Models;

namespace soverance.com.Ethereal.Controllers
{
    [Area("Ethereal")]
    public class EtherealController : Controller
    {
        [Route("ethereal")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("ethereal/eula")]
        public IActionResult EULA()
        {
            return View();
        }

        [Route("ethereal/versions")]
        public IActionResult Versions()
        {
            return View();
        }

        [Route("ethereal/guide")]
        public IActionResult Guide()
        {
            return View();
        }

        [Route("ethereal/guide/world")]
        public IActionResult World()
        {
            return View();
        }

        [Route("ethereal/guide/systems")]
        public IActionResult Systems()
        {
            return View();
        }

        [Route("ethereal/guide/loot")]
        public IActionResult Loot()
        {
            return View();
        }

        [Route("ethereal/guide/ansel")]
        public IActionResult Ansel()
        {
            return View();
        }
    }
}
