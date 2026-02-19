using Catalog.Service.Commands;
using Catalog.Service.Mappers;
using Catalog.Service.Repositories.Interfaces;
using MediatR;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Handlers
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {
        private readonly IProductRepository _prepository;
        public CreateProductCommandHandler(IProductRepository prepositor)
        {
            _prepository = prepositor;
        }
        public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var brand = await _prepository.GetBrandByBrandIdAsync(request.BrandId);
            var type = await _prepository.GetTypeByTypeIdAsync(request.TypeId);

            if (brand is null || type is null)
            {
                throw new Exception("Invalid Brand or Type");
            }

            var productEntity = request.ToEntity(brand, type);
            var createdProduct = await _prepository.CreateProduct(productEntity);
            return createdProduct.ToResponse();
        }
    }
}
