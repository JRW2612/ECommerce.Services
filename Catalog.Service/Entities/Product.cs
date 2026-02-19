using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Service.Entities
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }

        public string? Summary { get; set; }
        public string? Description { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string? ImageFile { get; set; }
        public ProductBrand? Brand { get; set; }
        public ProductType? Type { get; set; }

        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
