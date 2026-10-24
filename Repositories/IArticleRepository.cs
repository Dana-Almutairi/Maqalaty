
using Maqalaty.Models;

namespace Maqalaty.Repositories
{
    public interface IArticleRepository
    {
        Task<List<Article>> GetPublishedArticlesAsync();

        // Search published articles by title
        Task<List<Article>> SearchPublishedArticlesAsync(string searchTerm);

        // Filter published articles by category
        Task<List<Article>> GetPublishedArticlesByCategoryAsync(int categoryId);

        Task<Article?> GetPublishedArticleByIdAsync(int id);

        Task<Article?> GetByIdAsync(int id);

        Task AddAsync(Article article);

        Task UpdateAsync(Article article);

        Task DeleteAsync(Article article);
    }
}
