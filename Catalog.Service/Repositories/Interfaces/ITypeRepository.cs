using Catalog.Service.Entities;

namespace Catalog.Service.Repositories.Interfaces
{
    public interface ITypeRepository
    {
        Task<IEnumerable<ProductType>> GetAllTypes();
        Task<ProductType> GetTypeByIdAsync(string id);
    }
}
