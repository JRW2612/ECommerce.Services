using Discount.Grpc.Protos;

namespace Basket.Service.GrpcService
{
    public class DiscountGrcpService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _dprotoServiceClient;

        public DiscountGrcpService(DiscountProtoService.DiscountProtoServiceClient dprotoServiceClient)
        {
            _dprotoServiceClient = dprotoServiceClient;
        }

        public async Task<CouponModel> GetDiscountAsync(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };
            var call = _dprotoServiceClient.GetDiscountAsync(discountRequest);
            return await call.ResponseAsync;
        }
    }
}
