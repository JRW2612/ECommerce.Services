using MediatR;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Queries
{
    public record GetProductByIdQuery(string Id) : IRequest<ProductResponse>;

}
