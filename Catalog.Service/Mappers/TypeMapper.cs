using Catalog.Service.Entities;
using static Catalog.Service.Responses.Responses;

namespace Catalog.Service.Mappers
{
    public static class TypeExtension
    {
        public static TypeResponse ToResponse(this ProductType response)
        {
            return new TypeResponse
            {
                Id = response.Id,
                Name = response.Name
            };
        }

        public static IList<TypeResponse> ToResponseList(this IEnumerable<ProductType> types)
        {
            return types.Select(b => b.ToResponse()).ToList();
        }
    }
}
