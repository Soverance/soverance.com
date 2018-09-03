using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using soverance.com.Models;

namespace soverance.com.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly DatabaseContext _context;

        public SelectList CategoryDropDownList { get; set; }

        public BlogController(DatabaseContext context)
        {
            _context = context;
        }
        
        // GET: /Blog
        [AllowAnonymous]
        [Route("blog")]        
        public async Task<IActionResult> Index()
        {
            ViewBag.AllPosts = await _context.Post.ToListAsync();
            
            return View(await _context.Category.ToListAsync());
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
        [Route("blog/{slug}")]
        public async Task<IActionResult> ViewPost(string slug)
        {
            if (slug == null)
            {
                return NotFound();
            }

            var Post = await _context.Post.FirstOrDefaultAsync(m => m.Slug == slug);
            if (Post == null)
            {
                return NotFound();
            }
            if (Post != null)
            {
                var Category = await _context.Category.FirstOrDefaultAsync(m => m.CategoryId == Post.CategoryId);  // get the post's category object                
                Post.Category = Category;  // store the category object in the model for later use                
                //string url = "/blog/" + slug;
                //return new RedirectResult(url, true, true);
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
                                    <img src=""~/images/post/post-small-1.jpg"" alt="""" class=""img-responsive mlr-auto mb20"">
                                </p>
                                <p class=""col-lg-4 col-md-4 col-sm-12 col-sm-12 m0"">
                                    <img src=""~/images/post/post-small-2.jpg"" alt="""" class=""img-responsive mlr-auto mb20"">
                                </p>
                                <p class=""col-lg-4 col-md-4 col-sm-12 col-sm-12 m0"">
                                    <img src=""~/images/post/post-small-3.jpg"" alt="""" class=""img-responsive mlr-auto mb20"">
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
            ViewBag.CategoryDropDownList = new SelectList(await _context.Category.ToListAsync(), "CategoryId", "CategoryName");

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
        public async Task<IActionResult> CreatePost([Bind("PostId,CategoryId,Slug,Date,Image,Title,Content,Author")] Post Post)
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

            ViewBag.CategoryDropDownList = new SelectList(await _context.Category.ToListAsync(), "CategoryId", "CategoryName");
            
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
        public async Task<IActionResult> EditPost(int id, [Bind("PostId,CategoryId,Slug,Date,Image,Title,Content,Author")] Post Post)
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
                return RedirectToAction(nameof(Index));
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
        [HttpPost, ActionName("DeleteCategory")]
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
        [HttpPost, ActionName("DeletePost")]
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
