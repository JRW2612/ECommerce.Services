using MediatR;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Queries
{
    public record GetProductByNameQuery(string name) : IRequest<IList<ProductResponse>>;

}
