using Catalog.Service.Entities;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Mappers
{
    public static class BrandMapper
    {
        public static BrandResponse ToResponse(this ProductBrand response)
        {
            return new BrandResponse
            {
                Id = response.Id,
                Name = response.Name
            };
        }

        public static IList<BrandResponse> ToResponseList(this IEnumerable<ProductBrand> brands)
        {
            return brands.Select(b => b.ToResponse()).ToList();
        }



    }
}
