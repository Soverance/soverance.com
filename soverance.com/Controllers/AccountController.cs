using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using soverance.com.Models;

namespace soverance.com.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, OpenIdConnectDefaults.AuthenticationScheme);

        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignOut()
        {
            var callbackUrl = Url.Action(nameof(SignedOut), "Account", values: null, protocol: Request.Scheme);
            return SignOut(new AuthenticationProperties { RedirectUri = callbackUrl }, CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignedOut()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

