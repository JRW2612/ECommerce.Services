using Discount.Service.Data.DTOs;
using Discount.Service.Mappers;
using Discount.Service.Queries;
using Discount.Service.Repositories.Interface;
using Grpc.Core;
using MediatR;

namespace Discount.Service.Handlers
{
    public class GetDiscountHandler : IRequestHandler<GetDiscountQuery, CouponDto>
    {
        private readonly IDiscountRepository _dRepository;

        public GetDiscountHandler(IDiscountRepository dRepository)
        {
            _dRepository = dRepository;
        }


        public async Task<CouponDto> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ProductName))
            {
                var validationError = new Dictionary<string, string>
                {
                    {"ProductName","ProductName cannot be null or empty"}
                };
            }
            var discount = await _dRepository.GetDiscount(request.ProductName);

            if (discount is null || request is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount for {request.ProductName} not found"));
            }
            return discount.ToDto();

        }
    }
}
