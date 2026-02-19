using Discount.Service.Commands;
using Discount.Service.Data.DTOs;
using Discount.Service.Helpers;
using Discount.Service.Mappers;
using Discount.Service.Repositories.Interface;
using Grpc.Core;
using MediatR;

namespace Discount.Service.Handlers
{
    public class UpdateDiscountHandler : IRequestHandler<UpdateDiscountCommand, CouponDto>
    {
        private readonly IDiscountRepository _dRepository;

        public UpdateDiscountHandler(IDiscountRepository dRepository)
        {
            _dRepository = dRepository;
        }
        public async Task<CouponDto> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var validationError = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(request.ProductName))
            {
                validationError["ProductName"] = "ProductName cannot be empty.";
            }
            if (string.IsNullOrWhiteSpace(request.Description))
            {
                validationError["Description"] = "Description cannot be empty.";
            }
            if (request.Amount <= 0)
            {
                validationError["Amount"] = "Amount must be greater than zero.";
            }
            if (validationError.Any())
            {
                throw GrpcErrorHelper.CreateValidationException(validationError);
            }

            //Convert To Entity
            var coupon = request.ToEntity();

            //save coupon to repository
            var updatedCoupon = await _dRepository.UpdateDiscount(coupon);
            if (!updatedCoupon)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount not updated..."));
            }
            //Convert To Dto
            return coupon.ToDto();
        }
    }
}
