using Maqalaty.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Maqalaty.Models;

namespace Maqalaty.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin Dashboard
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.TotalArticles = await _context.Articles.CountAsync();

            ViewBag.PublishedArticles = await _context.Articles
                .CountAsync(article => article.IsPublished);

            ViewBag.DraftArticles = await _context.Articles
                .CountAsync(article => !article.IsPublished);

            return View();
        }

        // Manage all articles, including drafts
        [HttpGet]
        public async Task<IActionResult> Articles()
        {
            var articles = await _context.Articles
                .Include(article => article.Category)
                .AsNoTracking()
                .OrderByDescending(article => article.CreatedAt)
                .ToListAsync();

            return View(articles);
        }

        // Manage categories
[HttpGet]
public async Task<IActionResult> Categories()
{
    var categories = await _context.Categories
        .AsNoTracking()
        .OrderBy(category => category.Name)
        .ToListAsync();

    return View(categories);
}

// Create category - GET
[HttpGet]
public IActionResult CreateCategory()
{
    return View();
}

// Create category - POST
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> CreateCategory(
    [Bind("Name")] Category category)
{
    category.Name = category.Name?.Trim() ?? string.Empty;

    // Validate category name
    if (string.IsNullOrWhiteSpace(category.Name))
    {
        ModelState.AddModelError(
            nameof(Category.Name),
            "Category name is required."
        );
    }

    if (ModelState.IsValid)
    {
        // Check whether the category already exists
        bool categoryExists = await _context.Categories
            .AnyAsync(existing =>
                existing.Name == category.Name);

        if (categoryExists)
        {
            ModelState.AddModelError(
                nameof(Category.Name),
                "This category already exists."
            );
        }
    }

    if (!ModelState.IsValid)
    {
        return View(category);
    }

    try
    {
        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Categories));
    }
    catch (DbUpdateException ex)
        when (ex.InnerException is SqlException sqlException &&
              (sqlException.Number == 2601 ||
               sqlException.Number == 2627))
    {
        // Handle unique constraint violations
        ModelState.AddModelError(
            nameof(Category.Name),
            "This category already exists."
        );

        return View(category);
    }
}

// Delete category - GET
[HttpGet]
public async Task<IActionResult> DeleteCategory(int id)
{
    var category = await _context.Categories
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == id);

    if (category == null)
    {
        return NotFound();
    }

    return View(category);
}

// Delete category - POST
[HttpPost]
[ActionName("DeleteCategory")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteCategoryConfirmed(int id)
{
    var category = await _context.Categories
        .FirstOrDefaultAsync(c => c.Id == id);

    if (category == null)
    {
        return NotFound();
    }

    // Remove the category association from its articles
    var relatedArticles = await _context.Articles
        .Where(a => a.CategoryId == id)
        .ToListAsync();

    foreach (var article in relatedArticles)
    {
        article.CategoryId = null;
    }

    _context.Categories.Remove(category);

    await _context.SaveChangesAsync();

    return RedirectToAction(nameof(Categories));
}
    }
}