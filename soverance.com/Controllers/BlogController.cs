using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using soverance.com.Models;

namespace soverance.com.Controllers
{
    public class BlogController : Controller
    {
        private readonly DatabaseContext _context;

        public SelectList CategoryDropDownList { get; set; }

        public BlogController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: /Blog
        public async Task<IActionResult> Index()
        {
            ViewBag.AllPosts = await _context.Post.ToListAsync();

            return View(await _context.Category.ToListAsync());
        }

        // GET: /Blog/ViewCategory/5
        public async Task<IActionResult> ViewCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Category = await _context.Category
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (Category == null)
            {
                return NotFound();
            }

            return View(Category);
        }

        // GET: /Blog/CreateCategory
        public IActionResult CreateCategory()
        {
            return View();
        }

        // GET: /Blog/CreatePost
        public async Task<IActionResult> CreatePost()
        {
            //var list = new List<Category>();

            //// this section collects all the categories from the database, and adds them to a list that can be used later as a dropdown menu in the CreatePost page
            //using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            //using (SqlCommand command = new SqlCommand("SELECT CategoryId, CategoryName FROM Category", connection))
            //{
            //    connection.Open();
            //    using (SqlDataReader reader = command.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            list.Add(new Category
            //            {
            //                CategoryId = reader.GetInt32(0),
            //                CategoryName = reader.GetString(1)
            //            });
            //        }
            //    }
            //}

            //ViewBag.CategoryDropDownList = new SelectList(list, "CategoryId", "CategoryName");  // store a list of categories to use as dropdown list

            ViewBag.CategoryDropDownList = new SelectList(await _context.Category.ToListAsync(), "CategoryId", "CategoryName");

            // store current date - formatted as "2/27/2009"
            // it's important that we store the date in this format so that we can easily sort the posts by date
            DateTime time = DateTime.Now;
            ViewBag.CurrentDate = time.ToShortDateString();  

            return View();
        }

        // POST: /Blog/CreateCategory
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> CreatePost([Bind("PostId,CategoryId,Date,Title,Content,Author")] Post Post)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(Post);
        }

        // GET: Blog/EditCategory/5
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
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> EditPost(int id, [Bind("PostId,CategoryId,Date,Title,Content,Author")] Post Post)
        {
            if (id != Post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Category = await _context.Category
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (Category == null)
            {
                return NotFound();
            }

            return View(Category);
        }

        // GET: Blog/DeletePost/5
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Post = await _context.Post
                .FirstOrDefaultAsync(p => p.PostId == id);
            if (Post == null)
            {
                return NotFound();
            }

            return View(Post);
        }

        // POST: Blog/DeleteCategory/5
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
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
