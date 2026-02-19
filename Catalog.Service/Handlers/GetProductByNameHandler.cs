using Catalog.Service.Mappers;
using Catalog.Service.Queries;
using Catalog.Service.Repositories.Interfaces;
using MediatR;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Handlers
{
    public class GetProductByNameHandler : IRequestHandler<GetProductByNameQuery, IList<ProductResponse>>
    {
        private readonly IProductRepository _prepository;
        public GetProductByNameHandler(IProductRepository prepositor)
        {
            _prepository = prepositor;
        }

        public async Task<IList<ProductResponse>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
        {
            //  throw new NotImplementedException();
            var result = await _prepository.GetProductByNameAsync(request.name);
            var resultResponse = result.ToResponseList().ToList();
            return resultResponse;
        }

    }
}
