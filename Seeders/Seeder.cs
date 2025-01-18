using Tourkit.Models;

namespace Tourkit.Seeders
{
    public class Seeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Categories.Any() && !context.Products.Any())
            {

                var categories = Enumerable.Range(1, 20).Select(i => new Category
                {
                    Name = $"Category {i}",
                    Created = DateTime.Now.AddDays(-i)
                }).ToList();

                context.Categories.AddRange(categories);
                context.SaveChanges();

                var categoryIds = categories.Select(c => c.Id).ToList();

                var random = new Random();
                var products = Enumerable.Range(1, 10000).Select(i => new Product
                {
                    Name = $"Product {i}",
                    Price = random.Next(10, 1000),
                    CategoryId = categoryIds[random.Next(categoryIds.Count)]
                }).ToList();

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
