using Discount.Service.Commands;
using Discount.Service.Data.DTOs;
using Discount.Service.Helpers;
using Discount.Service.Mappers;
using Discount.Service.Repositories.Interface;
using MediatR;

namespace Discount.Service.Handlers
{
    public class CreateDiscountHandler : IRequestHandler<CreateDiscountCommand, CouponDto>
    {
        private readonly IDiscountRepository _dRepository;

        public CreateDiscountHandler(IDiscountRepository dRepository)
        {
            _dRepository = dRepository;
        }

        public async Task<CouponDto> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
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
            await _dRepository.CreateDiscount(coupon);
            //Convert To Dto
            var couponDto = coupon.ToDto();
            return couponDto;
        }
    }
}
