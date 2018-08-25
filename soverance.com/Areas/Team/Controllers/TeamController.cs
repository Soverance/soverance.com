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

namespace soverance.com.Team.Controllers
{
    [Area("Team")]
    public class TeamController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ScottMcCutchen()
        {
            return View();
        }
    }
}
