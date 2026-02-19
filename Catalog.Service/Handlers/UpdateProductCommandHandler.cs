using Catalog.Service.Commands;
using Catalog.Service.Mappers;
using Catalog.Service.Repositories.Interfaces;
using MediatR;

namespace Catalog.Service.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _prepository;
        public UpdateProductCommandHandler(IProductRepository prepositor)
        {
            _prepository = prepositor;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingData = await _prepository.GetProductByIdAsync(request.Id!);
            if (existingData is null)
            {
                throw new Exception("Product not found");
            }
            var brand = await _prepository.GetBrandByBrandIdAsync(request.BrandId);
            var type = await _prepository.GetTypeByTypeIdAsync(request.TypeId);

            if (brand is null || type is null)
            {
                throw new Exception("Invalid Brand or Type");
            }

            var updateProduct = request.ToUpdateEntity(existingData, brand, type);
            var result = await _prepository.UpdateProduct(updateProduct);
            return result;
        }
    }
}
