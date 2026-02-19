using Catalog.Service.Specifications;
using MediatR;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Queries
{
    public record GetAllProductQuery(CatalogSpecParams specParams) : IRequest<Pagination<ProductResponse>>;

}
