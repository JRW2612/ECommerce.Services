using Catalog.Service.Mappers;
using Catalog.Service.Queries;
using Catalog.Service.Repositories.Interfaces;
using MediatR;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Handlers
{
    public class GetAllBrandHandler : IRequestHandler<GetAllBrandQuery, IList<BrandResponse>>
    {
        private readonly IBrandRepository _brepository;
        public GetAllBrandHandler(IBrandRepository brepository)
        {
            _brepository = brepository;
        }
        public async Task<IList<BrandResponse>> Handle(GetAllBrandQuery request, CancellationToken cancellationToken)
        {
            var brandList = await _brepository.GetAllBrands();
            return brandList.ToResponseList();
        }
    }
}
