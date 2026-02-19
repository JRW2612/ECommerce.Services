using MediatR;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Queries
{
    public record GetAllTypeQuery : IRequest<IList<TypeResponse>>;

}
