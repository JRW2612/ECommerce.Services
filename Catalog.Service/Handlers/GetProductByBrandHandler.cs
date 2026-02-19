using Catalog.Service.Mappers;
using Catalog.Service.Queries;
using Catalog.Service.Repositories.Interfaces;
using MediatR;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Handlers
{
    public class GetProductByBrandHandler : IRequestHandler<GetProductByBrandQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository _pRepository;
        public GetProductByBrandHandler(IProductRepository pRepository)
        {
            _pRepository = pRepository;
        }

        public async Task<IList<ProductResponse>> Handle(GetProductByBrandQuery request, CancellationToken cancellationToken)
        {
            var result = await _pRepository.GetProductByBrandAsync(request.name);
            return result.ToResponseList().ToList();
        }
    }
}
