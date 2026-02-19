using Catalog.Service.Mappers;
using Catalog.Service.Queries;
using Catalog.Service.Repositories.Interfaces;
using Catalog.Service.Specifications;
using MediatR;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Handlers
{
    public class GetAllProductHandler : IRequestHandler<GetAllProductQuery, Pagination<ProductResponse>>
    {
        private readonly IProductRepository _prepository;
        public GetAllProductHandler(IProductRepository prepository)
        {
            _prepository = prepository;
        }

        public async Task<Pagination<ProductResponse>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var productList = await _prepository.GetProducts(request.specParams);

            // Map the Pagination<Product> to Pagination<ProductResponse>
            var productResponseList = productList.ToResponse();

            return productResponseList;
        }
    }
}
