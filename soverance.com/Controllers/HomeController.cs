using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using soverance.com.Models;


namespace soverance.com.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DatabaseContext SovDatabaseContext;
        private readonly IOptions<SecretConfig> SovSecretConfig;
        private readonly IOptions<MailConfig> SovMailConfig;

        public HomeController(DatabaseContext _SovDatabaseContext, IOptions<SecretConfig> _SovSecretConfig, IOptions<MailConfig> _SovMailConfig)
        {
            // construct the configuration objects for this controller
            SovDatabaseContext = _SovDatabaseContext;
            SovSecretConfig = _SovSecretConfig;
            SovMailConfig = _SovMailConfig;
        }

        [AllowAnonymous]
        [Route("")]
        [Route("home")]
        [Route("home/index")]
        public async Task<IActionResult> Index()
        {
            // BLOG POSTS
            List<Post> LatestThree = await SovDatabaseContext.Post.OrderByDescending(o => o.PostId).Take(3).ToListAsync();
            ViewBag.LatestThree = LatestThree;
            foreach (Post post in LatestThree)
            {
                var Category = await SovDatabaseContext.Category.FirstOrDefaultAsync(m => m.CategoryId == post.CategoryId);  // get the post's category object                
                post.Category = Category;  // store the category values in the post's model so we can display the name
            }

            // YOUTUBE VIDEOS
            string GoogleApiKey = SovSecretConfig.Value.GoogleApiKey;
            List<YouTubeData> VideoList = YouTubeModel.GetVideos(GoogleApiKey, true);
            ViewBag.LatestVideos = VideoList.Take(4);
            ViewBag.TotalChannelViews = YouTubeModel.GetChannelTotalViews(GoogleApiKey);
            ViewBag.TotalSubscribers = YouTubeModel.GetChannelSubscriberCount(GoogleApiKey);            
            List<YouTubeData> TutorialList = YouTubeModel.GetVideos(GoogleApiKey, false, "PLXZQqd9R-mFHH3cBvJIffMUUWQjl9h1Kh");
            ViewBag.TutorialPlaylist = TutorialList.Take(4);
            ViewBag.LatestTutorial = TutorialList[0];

            return View(VideoList);
        }

        [AllowAnonymous]
        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("contact")]
        public IActionResult Contact()
        {
            ViewBag.AzureMapsKey = SovSecretConfig.Value.AzureMapsKey;            

            return View();
        }

        [AllowAnonymous]
        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("faq")]
        public IActionResult FAQ()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("games")]
        public IActionResult Games()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("portfolio")]
        public IActionResult Portfolio()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("testimonials")]
        public IActionResult Testimonials()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("consulting")]
        public IActionResult Consulting()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("gallery")]
        public IActionResult Gallery()
        {
            return View();
        }

        [AllowAnonymous]
        [Route("error")]
        public IActionResult Error()
        {
            ViewData["statusCode"] = HttpContext.Response.StatusCode;
            ViewData["message"] = HttpContext.Features.Get<IExceptionHandlerFeature>().Error.Message;
            return View();
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("contact/results")]
        public IActionResult ContactResults(string ContactName, string ContactEmail, string ContactPhone, string Subject, string Message)
        {
            MailConfig mailConfig = new MailConfig
            {
                Server = SovMailConfig.Value.Server,
                Port = SovMailConfig.Value.Port,
                User = SovMailConfig.Value.User,
                Password = SovMailConfig.Value.Password,
                ContactName = ContactName,
                ContactEmail = ContactEmail,
                ContactPhone = ContactPhone,
                Subject = Subject,
                Message = Message
            };

            string ContactStatus = MailModel.SendContactEmail(mailConfig);
            ViewData["ContactStatus"] = ContactStatus;

            return View(mailConfig);
        }
    }
}
