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
            return View(await _context.Category.ToListAsync());
        }

        // GET: /Blog/DetailsCategory/5
        public async Task<IActionResult> DetailsCategory(int? id)
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
        public IActionResult CreatePost()
        {
            Post Post = new Post();
            var list = new List<Category>();

            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            using (SqlCommand command = new SqlCommand("SELECT CategoryId, CategoryName FROM Category", connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Category
                        {
                            CategoryId = reader.GetInt32(0),
                            CategoryName = reader.GetString(1)
                        });
                    }
                }
            }
            
            ViewBag.CategoryDropDownList = new SelectList(list, "CategoryId", "CategoryName");

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

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryId == id);
        }
    }
}
