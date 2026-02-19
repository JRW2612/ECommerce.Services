using Discount.Service.Commands;
using Discount.Service.Helpers;
using Discount.Service.Repositories.Interface;
using Grpc.Core;
using MediatR;

namespace Discount.Service.Handlers
{
    public class DeleteDiscountHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly IDiscountRepository _dRepository;

        public DeleteDiscountHandler(IDiscountRepository dRepository)
        {
            _dRepository = dRepository;
        }

        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var validationError = new Dictionary<string, string>();
            if (string.IsNullOrWhiteSpace(request.ProductName))
            {
                validationError["ProductName"] = "ProductName cannot be empty.";
            }
            if (validationError.Any())
            {
                throw GrpcErrorHelper.CreateValidationException(validationError);
            }


            //save coupon to repository
            var deletedCoupon = await _dRepository.DeleteDiscount(request.ProductName);
            if (!deletedCoupon)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount not updated..."));
            }
            return deletedCoupon;
        }
    }
}
