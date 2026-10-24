using Maqalaty.Models;
using Maqalaty.Repositories;

namespace Maqalaty.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<List<Article>> GetPublishedArticlesAsync()
        {
            return await _articleRepository.GetPublishedArticlesAsync();
        }

        // Search published articles by title
        public async Task<List<Article>> SearchPublishedArticlesAsync(string searchTerm)
        {
            return await _articleRepository.SearchPublishedArticlesAsync(searchTerm);
        }

        
        public async Task<List<Article>> GetPublishedArticlesByCategoryAsync(
            int categoryId)
        {
            return await _articleRepository
                .GetPublishedArticlesByCategoryAsync(categoryId);
        }


        public async Task<Article?> GetPublishedArticleByIdAsync(int id)
        {
            return await _articleRepository.GetPublishedArticleByIdAsync(id);
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            return await _articleRepository.GetByIdAsync(id);
        }

        public async Task CreateArticleAsync(Article article)
        {
            article.Id = 0;
            article.CreatedAt = DateTime.UtcNow;

            await _articleRepository.AddAsync(article);
        }

        public async Task UpdateArticleAsync(Article article)
        {
            await _articleRepository.UpdateAsync(article);
        }

        public async Task DeleteArticleAsync(Article article)
        {
            await _articleRepository.DeleteAsync(article);
        }
    }
}