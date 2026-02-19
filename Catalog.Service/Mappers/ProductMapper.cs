using Catalog.Service.Commands;
using Catalog.Service.Data.DTOs;
using Catalog.Service.Entities;
using Catalog.Service.Specifications;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Mappers
{
    public static class ProductMapper
    {
        public static ProductResponse ToResponse(this Product response)
        {
            if (response is null)
                return null;
            else
            {
                return new ProductResponse
                {
                    Id = response.Id,
                    Name = response.Name,
                    Description = response.Description,
                    Summary = response.Summary,
                    Price = response.Price,
                    ImageFile = response.ImageFile,
                    Brand = response.Brand,
                    Type = response.Type,
                    CreatedDate = response.CreatedDate
                };
            }
        }

        public static Pagination<ProductResponse> ToResponse(this Pagination<Product> pagination)
         => new Pagination<ProductResponse>
            (
                pagination.PageIndex,
                pagination.PageSize,
                pagination.Count,
                pagination.Data.Select(p => p.ToResponse()).ToList()
            );

        public static IEnumerable<ProductResponse> ToResponseList(this IEnumerable<Product>? products)
                    => (products ?? Enumerable.Empty<Product>())
                        .Where(p => p is not null)
                        .Select(p => p!.ToResponse()!);

        public static Product ToEntity(this CreateProductCommand cmd, ProductBrand productBrand, ProductType productType) =>
            new Product
            {
                Name = cmd.Name!,
                Description = cmd.Description!,
                Summary = cmd.Summary!,
                Price = cmd.Price,
                ImageFile = cmd.ImageFile!,
                Brand = productBrand,
                Type = productType,
                CreatedDate = DateTimeOffset.UtcNow
            };

        public static Product ToUpdateEntity(this UpdateProductCommand cmd, Product existingProduct, ProductBrand productBrand, ProductType productType)
        {
            return new Product
            {
                Id = existingProduct.Id!,
                Name = cmd.Name!,
                Description = cmd.Description!,
                Summary = cmd.Summary!,
                Price = cmd.Price,
                ImageFile = cmd.ImageFile!,
                Brand = productBrand,
                Type = productType,
                CreatedDate = existingProduct.CreatedDate
            };
        }

        public static ProductDto ToDto(this ProductResponse productResponse)
        {
            if (productResponse is null) return null;
            return new ProductDto(
                productResponse.Id,
                productResponse.Name,
                productResponse.Summary,
                productResponse.Description,
                productResponse.Price,
                productResponse.ImageFile,
                new BrandDto(productResponse.Id, productResponse.Brand.Name),
                new TypeDto(productResponse.Id, productResponse.Type.Name),
                DateTimeOffset.UtcNow
                );

        }

        public static UpdateProductCommand ToCommand(this UpdateProductDto updateProductDto, string id)
        {
            return new UpdateProductCommand
            {
                Id = id,
                Name = updateProductDto.Name,
                Summary = updateProductDto.Summary,
                Description = updateProductDto.Description,
                Price = updateProductDto.Price,
                ImageFile = updateProductDto.ImageFile,
                BrandId = updateProductDto.BrandId,
                TypeId = updateProductDto.TypeId
            };

        }

    }
}
