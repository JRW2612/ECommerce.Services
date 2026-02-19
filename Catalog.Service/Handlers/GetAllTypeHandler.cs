using Catalog.Service.Mappers;
using Catalog.Service.Queries;
using Catalog.Service.Repositories.Interfaces;
using MediatR;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Handlers
{
    public class GetAllTypeHandler : IRequestHandler<GetAllTypeQuery, IList<TypeResponse>>

    {
        private readonly ITypeRepository _trepository;
        public GetAllTypeHandler(ITypeRepository trepository)
        {
            _trepository = trepository;
        }

        public async Task<IList<TypeResponse>> Handle(GetAllTypeQuery request, CancellationToken cancellationToken)
        {
            var typeList = await _trepository.GetAllTypes();
            return typeList.ToResponseList();
        }
    }
}
