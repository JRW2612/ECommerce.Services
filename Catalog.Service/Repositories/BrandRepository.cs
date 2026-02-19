using Catalog.Service.Entities;
using Catalog.Service.Helpers;
using Catalog.Service.Repositories.Interfaces;
using MongoDB.Driver;

namespace Catalog.Service.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly IMongoCollection<ProductBrand> _brands;

        public BrandRepository(Constants constants)
        {
            var _connection = new MongoClient(constants.ConnectionString);
            var db = _connection.GetDatabase(constants.DatabaseName);
            _brands = db.GetCollection<ProductBrand>(constants.BrandsCollectionName);
            //var products = db.GetCollection<Product>(constants.ProductsCollectionName);
            //var types = db.GetCollection<ProductType>(constants.TypesCollectionName);
        }
        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _brands.FindAsync(_ => true).Result.ToListAsync();
        }

        public async Task<ProductBrand> GetBrandByIdAsync(string id)
        {
            // throw new NotImplementedException();
            return await _brands.FindAsync(b => b.Id == id).Result.FirstOrDefaultAsync();
        }
    }
}
