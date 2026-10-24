using Maqalaty.Data;
using Maqalaty.Models;
using Microsoft.EntityFrameworkCore;

namespace Maqalaty.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;

        public ArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all published articles with categories
        public async Task<List<Article>> GetPublishedArticlesAsync()
        {
            return await _context.Articles
                .Include(article => article.Category)
                .Where(article => article.IsPublished)
                .OrderByDescending(article => article.CreatedAt)
                .AsNoTracking()
                .ToListAsync();
        }

        // Search published articles by title
        public async Task<List<Article>> SearchPublishedArticlesAsync(
            string searchTerm)
        {
            IQueryable<Article> query = _context.Articles
                .Include(article => article.Category)
                .AsNoTracking()
                .Where(article => article.IsPublished);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string keyword = searchTerm.Trim();

                query = query.Where(article =>
                    article.Title.Contains(keyword));
            }

            return await query
                .OrderByDescending(article => article.CreatedAt)
                .ToListAsync();
        }

        
public async Task<List<Article>> GetPublishedArticlesByCategoryAsync(
    int categoryId)
{
    return await _context.Articles
        .Include(article => article.Category)
        .AsNoTracking()
        .Where(article =>
            article.IsPublished &&
            article.CategoryId == categoryId)
        .OrderByDescending(article => article.CreatedAt)
        .ToListAsync();
}


        // Get published article details with category
        public async Task<Article?> GetPublishedArticleByIdAsync(int id)
        {
            return await _context.Articles
                .Include(article => article.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(article =>
                    article.Id == id && article.IsPublished);
        }

        // Get article by ID for editing or deleting
        public async Task<Article?> GetByIdAsync(int id)
        {
            return await _context.Articles
                .FindAsync(id);
        }

        // Create article
        public async Task AddAsync(Article article)
        {
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
        }

        // Update article
        public async Task UpdateAsync(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
        }

        // Delete article
        public async Task DeleteAsync(Article article)
        {
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
        }
    }
}