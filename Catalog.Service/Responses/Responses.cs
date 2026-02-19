using Catalog.Service.Entities;

namespace Catalog.Service.Responses
{
    public class Responses
    {
        public class BrandResponse
        {
            public string? Id { get; init; }
            public string? Name { get; init; }
        }

        public class TypeResponse
        {
            public string? Id { get; init; }
            public string? Name { get; init; }
        }

        public class ProductResponse
        {
            public string? Id { get; init; }
            public string? Name { get; init; }
            public string? Summary { get; init; }
            public string? Description { get; init; }
            public decimal Price { get; init; }
            public string? ImageFile { get; init; }
            public ProductBrand? Brand { get; init; }
            public ProductType? Type { get; init; }
            public DateTimeOffset CreatedDate { get; init; }
        }
    }
}
