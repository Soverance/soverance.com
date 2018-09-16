using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using soverance.com.Models;

namespace soverance.com.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IOptions<SecretConfig> SovSecretConfig;

        public SelectList CategoryDropDownList { get; set; }

        public BlogController(DatabaseContext context, IOptions<SecretConfig> _SovSecretConfig)
        {
            _context = context;
            SovSecretConfig = _SovSecretConfig;
        }
        
        // GET: /Blog
        [AllowAnonymous]
        [Route("blog/{id?}")]        
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                id = 0;
            }

            List<Category> AllCategories = await _context.Category.ToListAsync();
            List<Post> LatestFive = await _context.Post.OrderByDescending(o => o.PostId).Take(5).ToListAsync();
            int PageCount = (int)Math.Ceiling((double)_context.Post.Count() / 10);
            int SkipPages = id.Value * 10;

            Pagination Pagination = new Pagination();
            Pagination.Posts = await _context.Post.OrderByDescending(o => o.PostId).Skip(SkipPages).Take(10).ToListAsync();
            Pagination.CurrentPageIndex = id.Value;
            Pagination.PageCount = PageCount;
            
            ViewBag.Pagination = Pagination;
            ViewBag.AllCategories = AllCategories;
            ViewBag.LatestFive = LatestFive;

            // an offset for the last page pagination link, since the blog index starts at zero, 
            // the last page icon will display as "2" to the user but would need to pass /blog/1/ to get routing to the last page
            ViewBag.LastPage = PageCount - 1;  

            // set and cap previous page value - consider refactoring.
            if (id.Value > 0)
            {
                ViewBag.PreviousPage = id.Value - 1;
            }
            else
            {
                ViewBag.PreviousPage = 0;
            }
            // set and cap next page value - consider refactoring.
            if (id.Value < PageCount)
            {
                ViewBag.NextPage = id.Value + 1;
            }
            else
            {
                ViewBag.NextPage = PageCount;
            }
            

            return View();
        }

        // GET: /Blog/ViewCategory/5
        [AllowAnonymous]
        [Route("blog/viewcategory/{id}")]
        public async Task<IActionResult> ViewCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Category = await _context.Category.FirstOrDefaultAsync(m => m.CategoryId == id);
            if (Category == null)
            {
                return NotFound();
            }

            return View(Category);
        }

        // GET: /Blog/ViewPost/5
        [AllowAnonymous]
        [Route("blog/post/{slug}")]
        public async Task<IActionResult> ViewPost(string slug)
        {
            if (slug == null)
            {
                return NotFound();
            }

            var Post = await _context.Post.FirstOrDefaultAsync(m => m.Slug == slug);

            // there is almost certainly a better way to get the previous and next post than what I'm doing here
            // I'm pretty sure this won't work when the PostId's become non-incremental
            // and it already has issues with the first and last blog post, where it's indexing becomes faulty
            List<Post> SurroundingPosts = await _context.Post.OrderBy(o => o.PostId).Skip(Math.Max(0, Post.PostId - 2)).Take(3).ToListAsync();
            ViewBag.SurroundingPosts = SurroundingPosts;

            if (Post == null)
            {
                return NotFound();
            }
            if (Post != null)
            {
                var Category = await _context.Category.FirstOrDefaultAsync(m => m.CategoryId == Post.CategoryId);  // get the post's category object                
                Post.Category = Category;  // store the category object in the model for later use  
            }
            
            return View(Post);
        }

        // GET: /Blog/CreateCategory
        [Route("blog/createcategory")]
        public IActionResult CreateCategory()
        {
            return View();
        }

        // GET: /Blog/CreatePost
        [Route("blog/createpost")]
        public async Task<IActionResult> CreatePost()
        {
            Post Post = new Post();
            string DefaultPostContent = @"<div class=""mt20"">
                                <p>This template contains some default text provided by Soverance Studios.  Replace this text with your own content in order to generate a new post. Be sure to keep the divs, classes, and other tagging in place in order to maintain visual formatting.</p>
                                <p>This template contains some default text provided by Soverance Studios.  Replace this text with your own content in order to generate a new post. Be sure to keep the divs, classes, and other tagging in place in order to maintain visual formatting.</p>
                            </div>
                            <div class=""post-quote mt120"">
                                <blockquote class=""p40 text-center"">
                                    <div class=""author-avatar"">
                                        <img src=""~/images/blog/status-author.jpg"" alt="""">
                                    </div>
                                    <div class=""table mb0 pb30 mt40"">
                                        <div class=""table-row"">
                                            <div class=""table-cell valign-middle"">
                                                <a href=""post.html"" class=""post-title fsize-24 fweight-700 font-agency color-white uppercase"">
                                                    Scott McCutchen
                                                </a>
                                                <div class=""fsize-14 fweight-700 uppercase color-1"">
                                                    Founder
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class=""text-container fsize-18 italic ptb30"">
                                        «Enter some quote text from the article.»
                                    </div>
                                </blockquote>
                            </div>
                            <p class=""mt60"">
                                This template contains some default text provided by Soverance Studios.  Replace this text with your own content in order to generate a new post. Be sure to keep the divs, classes, and other tagging in place in order to maintain visual formatting.
                            </p>
                            <div class=""clearfix"">
                                <p class=""col-lg-4 col-md-4 col-sm-12 col-sm-12 m0"">
                                    <a href=""/images/blog/ffxi/khim.jpg"" data-lightbox=""game - slides"">
                                               < img src = ""/images/blog/ffxi/fafnir.jpg"" alt = """" class=""img-responsive mlr-auto mb20"">
                                    </a>
                                </p>
                                <p class=""col-lg-4 col-md-4 col-sm-12 col-sm-12 m0"">
                                    <a href=""/images/blog/ffxi/khim.jpg"" data-lightbox=""game - slides"">
                                               < img src = ""/images/blog/ffxi/darkixion.jpg"" alt = """" class=""img-responsive mlr-auto mb20"">
                                    </a>
                                </p>
                                <p class=""col-lg-4 col-md-4 col-sm-12 col-sm-12 m0"">
                                    <a href=""/images/blog/ffxi/khim.jpg"" data-lightbox=""game - slides"">
                                               < img src = ""/images/blog/ffxi/khim.jpg"" alt = """" class=""img-responsive mlr-auto mb20"">
                                    </a>
                                </p>
                            </div>
                            <p class=""mt30"">
                                This template contains some default text provided by Soverance Studios.  Replace this text with your own content in order to generate a new post. Be sure to keep the divs, classes, and other tagging in place in order to maintain visual formatting.
                            </p>
                            <p>
                                This template contains some default text provided by Soverance Studios.  Replace this text with your own content in order to generate a new post. Be sure to keep the divs, classes, and other tagging in place in order to maintain visual formatting.
                            </p>
                            <ul class=""list-1"">
                                <li class=""color5"">
                                    Enter a list item.
                                </li>
                                <li class=""color5"">
                                    Enter a list item.
                                </li>
                                <li class=""color5"">
                                    Enter a list item.
                                </li>
                            </ul>";
            Post.Content = DefaultPostContent;  // we set the default value of post content so as to make creating new posts far easier

            // category dropdown array
            ViewBag.CategoryDropDownList = new SelectList(await _context.Category.ToListAsync(), "CategoryId", "CategoryName");

            // PostTypes dropdown array
            var PostTypes = new Dictionary<int, string>
            {
                { 1, "Static" },
                { 2, "Video" },
                { 3, "Slider" }
            };
            ViewBag.PostTypes = new SelectList(PostTypes, "Key", "Value");

            // store current date - formatted as "2/27/2009"
            // it's important that we store the date in this format so that we can easily sort the posts by date
            DateTime time = DateTime.Now;
            ViewBag.CurrentDate = time.ToShortDateString();  

            return View(Post);
        }

        // POST: /Blog/CreateCategory
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("blog/createcategory")]
        public async Task<IActionResult> CreateCategory([Bind("CategoryId,CategoryName")] Category Category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Category);
        }

        // POST: /Blog/CreatePost
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("blog/createpost")]
        public async Task<IActionResult> CreatePost([Bind("PostId,CategoryId,PostType,Slug,Date,Title,Description,Content,Author,VideoUrl,Slider1,Slider2,Slider3,PlaylistId")] Post Post)
        {
            if (ModelState.IsValid)
            {
                Post.Slug = UrlEncoder.SanitizeUrl(Post.Title);  // update slug
                _context.Add(Post);
                
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Post);
        }

        // GET: Blog/EditCategory/5
        [Route("blog/editcategory/{id}")]
        public async Task<IActionResult> EditCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Category = await _context.Category.FindAsync(id);
            if (Category == null)
            {
                return NotFound();
            }
            return View(Category);
        }

        // GET: Blog/EditPost/5
        [Route("blog/editpost/{id}")]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Category dropdown array
            ViewBag.CategoryDropDownList = new SelectList(await _context.Category.ToListAsync(), "CategoryId", "CategoryName");

            // PostTypes dropdown array
            var PostTypes = new Dictionary<int, string>
            {
                { 1, "Static" },
                { 2, "Video" },
                { 3, "Slider" }
            };
            ViewBag.PostTypes = new SelectList(PostTypes, "Key", "Value");

            var Post = await _context.Post.FindAsync(id);
            if (Post == null)
            {
                return NotFound();
            }
            return View(Post);
        }

        // POST: Blog/EditCategory/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("blog/editcategory/{id}")]
        public async Task<IActionResult> EditCategory(int id, [Bind("CategoryId,CategoryName")] Category Category)
        {
            if (id != Category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(Category.CategoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Category);
        }

        // POST: Blog/EditPost/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("blog/editpost/{id}")]
        public async Task<IActionResult> EditPost(int id, [Bind("PostId,CategoryId,PostType,Slug,Date,Title,Description,Content,Author,VideoUrl,Slider1,Slider2,Slider3,PlaylistId")] Post Post)
        {
            if (id != Post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Post.Slug = UrlEncoder.SanitizeUrl(Post.Title);  // update slug
                    _context.Update(Post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(Post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ViewPost", "Blog", new { slug = Post.Slug });
            }
            return View(Post);
        }

        // GET: Blog/DeleteCategory/5
        [Route("blog/deletecategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Category = await _context.Category.FirstOrDefaultAsync(m => m.CategoryId == id);
            if (Category == null)
            {
                return NotFound();
            }

            return View(Category);
        }

        // GET: Blog/DeletePost/5
        [Route("blog/deletepost/{id}")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Post = await _context.Post.FirstOrDefaultAsync(m => m.PostId == id);
            if (Post == null)
            {
                return NotFound();
            }

            return View(Post);
        }

        // POST: Blog/DeleteCategory/5
        [HttpPost, ActionName("ConfirmDeleteCategory")]
        [ValidateAntiForgeryToken]
        [Route("blog/deletecategoryconfirmed/{id}")]
        public async Task<IActionResult> DeleteCategoryConfirmed(int id)
        {
            var Category = await _context.Category.FindAsync(id);
            _context.Category.Remove(Category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Blog/DeletePost/5
        [HttpPost, ActionName("ConfirmDeletePost")]
        [ValidateAntiForgeryToken]
        [Route("blog/deletepostconfirmed/{id}")]
        public async Task<IActionResult> DeletePostConfirmed(int id)
        {
            var Post = await _context.Post.FindAsync(id);
            _context.Post.Remove(Post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.PostId == id);
        }
    }
}
