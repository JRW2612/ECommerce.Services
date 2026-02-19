using Catalog.Service.Entities;
using Catalog.Service.Specifications;

namespace Catalog.Service.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string productId);
        Task<IEnumerable<Product>> GetProductByNameAsync(string name);
        Task<IEnumerable<Product>> GetProductByBrandAsync(string name);
        Task<ProductBrand> GetBrandByBrandIdAsync(string Id);
        Task<ProductType> GetTypeByTypeIdAsync(string Id);
        Task<Product> CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string Id);
        Task<Pagination<Product>> GetProducts(CatalogSpecParams specParams);
    }
}
