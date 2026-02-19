using System.ComponentModel.DataAnnotations;

namespace Catalog.Service.Data.DTOs
{
    public record ProductDto
        (string Id,
        string Name,
          string Summary,
          string Description,
          decimal Price,
          string ImageFile,
          BrandDto Brand,
          TypeDto Type,
          DateTimeOffset CreatedDate
        );

    public record class CreateProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public string Description { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        [Required]
        public string ImageFile { get; set; }
        [Required]
        public string BrandId { get; set; }
        [Required]
        public string TypeId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }

    public record class UpdateProductDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Summary { get; set; }
        [Required]
        public string Description { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        [Required]
        public string ImageFile { get; set; }
        [Required]
        public string BrandId { get; set; }
        [Required]
        public string TypeId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
