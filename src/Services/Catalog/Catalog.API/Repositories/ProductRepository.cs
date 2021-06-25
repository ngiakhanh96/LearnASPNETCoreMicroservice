using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }


        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _catalogContext
                            .Products
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _catalogContext
                            .Products
                            .Find(p => p.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(string category)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Category, category);
            return await _catalogContext.Products.Find(filter).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _catalogContext.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
            var updateResult = await _catalogContext.Products.ReplaceOneAsync(filter, product);

            return updateResult.IsAcknowledged && updateResult.MatchedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            var deleteResult = await _catalogContext.Products.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
