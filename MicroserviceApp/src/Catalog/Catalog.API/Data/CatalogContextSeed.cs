using System.Collections.Generic;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            var existProducts = productCollection.Find(p => true).Any();
            if (!existProducts)
            {
                productCollection.InsertMany(GetPreConfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreConfiguredProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Name = "IPhone X",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "iphone-x.png",
                    Price = 950.00M,
                    Category = "Smart Phone"
                },
                new Product
                {
                    Name = "Samsung Galaxy S20",
                    Summary = "Summary",
                    Description = "Description",
                    ImageFile = "samsung.png",
                    Price = 850.00M,
                    Category = "Smart Phone"
                }
            };
        }
    }
}
