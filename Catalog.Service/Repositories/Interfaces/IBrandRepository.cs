using Catalog.Service.Entities;

namespace Catalog.Service.Repositories.Interfaces
{
    public interface IBrandRepository
    {
        Task<IEnumerable<ProductBrand>> GetAllBrands();
        Task<ProductBrand> GetBrandByIdAsync(string id);
    }
}
