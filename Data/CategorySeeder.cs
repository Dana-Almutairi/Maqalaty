using Maqalaty.Models;
using Microsoft.EntityFrameworkCore;

namespace Maqalaty.Data
{
    public static class CategorySeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context)
        {
            string[] categoryNames =
            {
                "Technology",
                "Programming",
                "Artificial Intelligence",
                "Business",
                "Science"
            };

            var existingNames = await context.Categories
                .Select(category => category.Name)
                .ToListAsync();

            foreach (var name in categoryNames)
            {
                if (!existingNames.Contains(
                    name,
                    StringComparer.OrdinalIgnoreCase))
                {
                    context.Categories.Add(new Category
                    {
                        Name = name
                    });
                }
            }

            await context.SaveChangesAsync();
        }
    }
}