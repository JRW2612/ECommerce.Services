using Catalog.Service.Entities;
using Catalog.Service.Helpers;
using Catalog.Service.Repositories.Interfaces;
using Catalog.Service.Specifications;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace Catalog.Service.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<ProductType> _types;
        private readonly IMongoCollection<ProductBrand> _brands;


        public ProductRepository(Constants constants)
        {
            var _connection = new MongoClient(constants.ConnectionString);
            var db = _connection.GetDatabase(constants.DatabaseName);
            _products = db.GetCollection<Product>(constants.ProductsCollectionName);
            _brands = db.GetCollection<ProductBrand>(constants.BrandsCollectionName);
            _types = db.GetCollection<ProductType>(constants.TypesCollectionName);
        }
        public async Task<Product> CreateProduct(Product product)
        {
            await _products.InsertOneAsync(product);
            return product;

        }

        public async Task<bool> DeleteProduct(string Id)
        {
            if (_products is not null)
            {
                var deleteResult = await _products.DeleteOneAsync(p => p.Id == Id);
                return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
            }
            else
            {
                throw new Exception("Products is not found.");
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _products.FindAsync(_ => true).Result.ToListAsync();
        }

        public async Task<ProductBrand> GetBrandByBrandIdAsync(string Id)
        {
            return await _brands.FindAsync(b => b.Id == Id).Result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByBrandAsync(string name)
        {
            return await _products.Find(p => p.Brand.Name == name).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string Id)
        {
            return await _products.FindAsync(b => b.Id == Id).Result.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByNameAsync(string name)
        {
            var filter = Builders<Product>.Filter.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression($".*{name}*.", "i"));
            return await _products.Find(filter).ToListAsync();
            //return await _products.Find(p => p.Name.ToLower() == name.ToLower()).ToListAsync();
        }

        public async Task<Pagination<Product>> GetProducts(CatalogSpecParams specParams)
        {
            //var builder = Builders<Product>.Filter;
            //var filter = builder.Empty;
            //if (string.IsNullOrEmpty(specParams.Search))
            //{
            //    filter &= builder.Where(p=>p.Name.Contains(specParams.Search));
            //}
            //if (string.IsNullOrEmpty(specParams.BrandId))
            //{
            //    filter &= builder.Where(p => p.Brand.Id.Contains(specParams.BrandId));
            //}
            //if (string.IsNullOrEmpty(specParams.TypeId))
            //{
            //    filter &= builder.Where(p => p.Type.Id.Contains(specParams.TypeId));
            //}

            //var totalItems = await _products.CountDocumentsAsync(filter);
            //var data=await ApplyDataFilters(specParams, filter);

            //return new Pagination<Product>(
            //    specParams.PageIndex,
            //    specParams.PageSize,
            //    (int)totalItems,
            //    data
            //    );
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;

            // Only add filters when the incoming params are non-empty
            if (!string.IsNullOrEmpty(specParams.Search))
            {
                var escaped = Regex.Escape(specParams.Search);
                var regex = new BsonRegularExpression($".*{escaped}.*", "i");
                filter &= builder.Regex(p => p.Name, regex);
            }
            if (!string.IsNullOrEmpty(specParams.BrandId))
            {
                var brandId = specParams.BrandId;
                filter &= builder.Where(p => p.Brand != null && p.Brand.Id != null && p.Brand.Id == brandId);
            }
            if (!string.IsNullOrEmpty(specParams.TypeId))
            {
                var typeId = specParams.TypeId;
                filter &= builder.Where(p => p.Type != null && p.Type.Id != null && p.Type.Id == typeId);
            }

            var totalItems = await _products.CountDocumentsAsync(filter);
            var data = await ApplyDataFilters(specParams, filter);

            return new Pagination<Product>(
                specParams.PageIndex,
                specParams.PageSize,
                (int)totalItems,
                data
                );
        }


        public async Task<ProductType> GetTypeByTypeIdAsync(string Id)
        {
            return await _types.FindAsync(b => b.Id == Id).Result.FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var result = await _products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }


        private async Task<IReadOnlyCollection<Product>> ApplyDataFilters(CatalogSpecParams specParams, FilterDefinition<Product> filter)
        {
            var sortdefn = Builders<Product>.Sort.Ascending("Name");
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        sortdefn = Builders<Product>.Sort.Ascending(p => p.Price);
                        break;
                    case "priceDesc":
                        sortdefn = Builders<Product>.Sort.Descending(p => p.Price);
                        break;
                    default:
                        sortdefn = Builders<Product>.Sort.Ascending("Name");
                        break;
                }
            }
            return await _products
                .Find(filter)
                .Sort(sortdefn)
                .Skip(specParams.PageSize * (specParams.PageIndex - 1))
                .Limit(specParams.PageSize)
                .ToListAsync();
        }

    }
}
