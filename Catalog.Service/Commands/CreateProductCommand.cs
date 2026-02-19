using MediatR;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Commands
{
    public class CreateProductCommand : IRequest<ProductResponse>
    {
        //  public string? Id { get; init; }
        public string? Name { get; init; }
        public string? Summary { get; init; }
        public string? Description { get; init; }
        public decimal Price { get; init; }
        public string? ImageFile { get; init; }
        public string? BrandId { get; init; }
        public string? TypeId { get; init; }
    }
}
