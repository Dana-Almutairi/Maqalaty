
using Maqalaty.Models;

namespace Maqalaty.Services
{
    public interface IArticleService
    {
        Task<List<Article>> GetPublishedArticlesAsync();

        // Search published articles by title
        Task<List<Article>> SearchPublishedArticlesAsync(string searchTerm);

        // Filter published articles by category
        Task<List<Article>> GetPublishedArticlesByCategoryAsync(int categoryId);

        Task<Article?> GetPublishedArticleByIdAsync(int id);

        Task<Article?> GetArticleByIdAsync(int id);

        Task CreateArticleAsync(Article article);

        Task UpdateArticleAsync(Article article);

        Task DeleteArticleAsync(Article article);
    }
}
