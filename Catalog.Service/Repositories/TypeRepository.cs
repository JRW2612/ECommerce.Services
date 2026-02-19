using Catalog.Service.Entities;
using Catalog.Service.Helpers;
using Catalog.Service.Repositories.Interfaces;
using MongoDB.Driver;

namespace Catalog.Service.Repositories
{
    public class TypeRepository : ITypeRepository
    {
        private readonly IMongoCollection<ProductType> _types;

        public TypeRepository(Constants constants)
        {
            var _connection = new MongoClient(constants.ConnectionString);
            var db = _connection.GetDatabase(constants.DatabaseName);
            _types = db.GetCollection<ProductType>(constants.TypesCollectionName);
        }
        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _types.FindAsync(_ => true).Result.ToListAsync();
        }

        public async Task<ProductType> GetTypeByIdAsync(string id)
        {
            //  throw new NotImplementedException();
            return await _types.FindAsync(b => b.Id == id).Result.FirstOrDefaultAsync();

        }
    }
}
