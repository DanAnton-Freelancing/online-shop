using System.Collections.Generic;
using System.Linq;
using OnlineShop.Secondary.Ports.DataContracts;

namespace OnlineShop.Secondary.Adapters.Initializer
{
    public static class DatabaseInitializer
    {
        private static readonly List<Category> Categories = new()
        {
                                                                new Category
                                                                {
                                                                    Name = "Games",
                                                                    Products = new List<Product>
                                                                               {
                                                                                   new()
                                                                                   {
                                                                                       Name = "Need For Speed",
                                                                                       AvailableQuantity = 50,
                                                                                       Price = (decimal?) 150.32
                                                                                   },

                                                                                   new()
                                                                                   {
                                                                                       Name = "Fifa 19",
                                                                                       AvailableQuantity = 100,
                                                                                       Price = (decimal?) 254.56
                                                                                   }
                                                                               }
                                                                },

                                                                new Category
                                                                {
                                                                    Name = "Electronics",
                                                                    Products = new List<Product>
                                                                               {
                                                                                   new()
                                                                                   {
                                                                                       Name = "Coffee machine",
                                                                                       AvailableQuantity = 60,
                                                                                       Price = (decimal?) 5000.45
                                                                                   }
                                                                               }
                                                                },

                                                                new Category
                                                                {
                                                                    Name = "Books",
                                                                    Products = new List<Product>
                                                                               {
                                                                                   new()
                                                                                   {
                                                                                       Name =
                                                                                           "Building maintainable software",
                                                                                       AvailableQuantity = 1000,
                                                                                       Price = (decimal?) 78.99
                                                                                   }
                                                                               }
                                                                },
                                                                new Category
                                                                {
                                                                    Name = "Food"
                                                                }
                                                            };

        public static void Seed(DatabaseContext dbContext)
        {
            foreach (var category in Categories.Where(category => !dbContext.Categories.Any(r => r.Name == category.Name)))
            {
                dbContext.Categories.Add(category);
            }

            dbContext.SaveChanges();
        }
    }
}