using Catalog.Service.Mappers;
using Catalog.Service.Queries;
using Catalog.Service.Repositories.Interfaces;
using MediatR;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Handlers
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        private readonly IProductRepository _prepository;
        public GetProductByIdHandler(IProductRepository prepositor)
        {
            _prepository = prepositor;
        }
        public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // throw new NotImplementedException();
            var result = await _prepository.GetProductByIdAsync(request.Id);
            var resultResponse = result.ToResponse();
            return resultResponse;

        }
    }
}
