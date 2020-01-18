using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using soverance.com.Helpers;
using soverance.com.Models;


namespace soverance.com.Controllers
{
    public class ClientController : Controller
    {
        private readonly ILogger _logger;

        public ClientController(ILogger<ClientController> logger)
        {
            _logger = logger;
        }

        // this function creates a short hex string using a secret salt plus an expiration time
        public static string GenerateExpiryHash(DateTime expiry)
        {
            const string salt = "ja45go9iRE";
            byte[] bytes = Encoding.UTF8.GetBytes(salt + expiry.ToString("s"));
            using (var sha = System.Security.Cryptography.SHA1.Create())
                return string.Concat(sha.ComputeHash(bytes).Select(b => b.ToString("x2"))).Substring(8);
        }

        public static string GenerateClientPassword(string user)
        {
            string password = Password.Generate(14, 0, user);
            return password;
        }

        public static string GenerateClientLink(string user)
        {
            DateTime expires = DateTime.Now + TimeSpan.FromDays(1);  // determine how long until the link expires
            string hash = GenerateExpiryHash(expires);
            string link = string.Format("https://soverance.com/Client?user={0}&exp={1}&k={2}", user, expires.ToString("s"), hash);
            return link;
        }

        [AllowAnonymous]
        [Route("client")]
        // Load client's profile.
        public IActionResult Client()
        {
            if (User.Identity.IsAuthenticated)
            {

            }
            else
            {                
                DateTime expires = DateTime.Parse(Request.Query["exp"]);
                ViewData["Message"] = expires.ToString();
                string hash = GenerateExpiryHash(expires);
                if (Request.Query["k"] == hash)
                {
                    if (expires < DateTime.UtcNow)
                    {
                        // Link has expired
                        ViewData["Message"] = "This link is expired.";
                    }
                    else
                    {
                        string user = Request.Query["user"].ToString();
                        string password = GenerateClientPassword(user);
                        ViewData["User"] = user;
                        ViewData["Password"] = password;
                        string msg = "This link is only good for 24 hours, and will expire on " + expires.ToString() + Environment.NewLine;
                        msg += Environment.NewLine + "Please securely record your credentials elsewhere for future reference.";
                        ViewData["Message"] = msg;
                    }
                }
                else
                {
                    // Invalid link
                    ViewData["Message"] = "This link is invalid.";
                }
            }

            return View();
        }

        [AllowAnonymous]
        [Route("client/generatelink")]
        // Load user's profile.
        public IActionResult GenerateLink(string user)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(user))
                {
                    try
                    {
                        ViewData["User"] = user;
                        string password = GenerateClientPassword(user);
                        ViewData["Password"] = password;
                        ViewData["Link"] = GenerateClientLink(user);
                        ViewData["Message"] = "Send the above link to the client. DO NOT SHARE IT WITH ANYONE ELSE!";

                    }
                    catch (Exception ex)
                    {
                        ViewData["Message"] = ex.ToString();
                    }
                }
            }

            return View();
        }
    }
}
