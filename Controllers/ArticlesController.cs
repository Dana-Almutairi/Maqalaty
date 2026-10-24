using Maqalaty.Data;
using Maqalaty.Models;
using Maqalaty.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Maqalaty.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ApplicationDbContext _context;

        public ArticlesController(
            IArticleService articleService,
            ApplicationDbContext context)
        {
            _articleService = articleService;
            _context = context;
        }

        // Load categories into dropdown
        private async Task LoadCategoriesAsync()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .OrderBy(category => category.Name)
                .ToListAsync();

            ViewBag.Categories = new SelectList(
                categories,
                "Id",
                "Name"
            );
        }

        // Validate selected category
        private async Task<bool> IsValidCategoryAsync(int? categoryId)
        {
            if (!categoryId.HasValue)
            {
                return true;
            }

            return await _context.Categories
                .AnyAsync(category => category.Id == categoryId.Value);
        }

        // Display and search published articles
        [HttpGet]
        
        // Display, search, and filter published articles
        [HttpGet]
        public async Task<IActionResult> Index(
            string? searchTerm,
            int? categoryId)
        {
            var keyword = searchTerm?.Trim();

            ViewData["SearchTerm"] = keyword;
            ViewData["SelectedCategoryId"] = categoryId;

            var categories = await _context.Categories
                .AsNoTracking()
                .OrderBy(category => category.Name)
                .ToListAsync();

            ViewBag.Categories = new SelectList(
                categories,
                "Id",
                "Name",
                categoryId
            );

            List<Article> articles;

            if (categoryId.HasValue)
            {
                articles = await _articleService
                    .GetPublishedArticlesByCategoryAsync(categoryId.Value);

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    articles = articles
                        .Where(article =>
                            article.Title.Contains(
                                keyword,
                                StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
            }
            else if (!string.IsNullOrWhiteSpace(keyword))
            {
                articles = await _articleService
                    .SearchPublishedArticlesAsync(keyword);
            }
            else
            {
                articles = await _articleService
                    .GetPublishedArticlesAsync();
            }

            return View(articles);
        }


        // Display published article details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var article =
                await _articleService.GetPublishedArticleByIdAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // Create article - GET
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            await LoadCategoriesAsync();

            return View();
        }

        // Create article - POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Title,Content,Author,IsPublished,CategoryId")]
            Article article)
        {
            if (!await IsValidCategoryAsync(article.CategoryId))
            {
                ModelState.AddModelError(
                    nameof(Article.CategoryId),
                    "Please select a valid category."
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();

                return View(article);
            }

            await _articleService.CreateArticleAsync(article);

            return RedirectToAction(nameof(Index));
        }

        // Edit article - GET
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var article =
                await _articleService.GetArticleByIdAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            await LoadCategoriesAsync();

            return View(article);
        }

        // Edit article - POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Content,Author,IsPublished,CategoryId")]
            Article updatedArticle)
        {
            if (id != updatedArticle.Id)
            {
                return BadRequest();
            }

            if (!await IsValidCategoryAsync(updatedArticle.CategoryId))
            {
                ModelState.AddModelError(
                    nameof(Article.CategoryId),
                    "Please select a valid category."
                );
            }

            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();

                return View(updatedArticle);
            }

            var article =
                await _articleService.GetArticleByIdAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            article.Title = updatedArticle.Title;
            article.Content = updatedArticle.Content;
            article.Author = updatedArticle.Author;
            article.IsPublished = updatedArticle.IsPublished;
            article.CategoryId = updatedArticle.CategoryId;

            await _articleService.UpdateArticleAsync(article);

            return RedirectToAction(nameof(Index));
        }

        // Delete article - GET
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var article =
                await _articleService.GetArticleByIdAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // Delete article - POST
        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article =
                await _articleService.GetArticleByIdAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            await _articleService.DeleteArticleAsync(article);

            return RedirectToAction(nameof(Index));
        }
    }
}