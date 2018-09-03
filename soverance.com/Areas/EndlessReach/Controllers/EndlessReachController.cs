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

namespace soverance.com.EndlessReach.Controllers
{
    [Authorize]
    [Area("EndlessReach")]
    public class EndlessReachController : Controller
    {
        [AllowAnonymous]
        [Route("endlessreach")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
