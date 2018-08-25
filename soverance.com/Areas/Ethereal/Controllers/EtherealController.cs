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
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Guide()
        {
            return View();
        }

        public IActionResult EULA()
        {
            return View();
        }
    }
}
