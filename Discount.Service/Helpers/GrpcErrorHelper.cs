using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using GoogleStatus = Google.Rpc.Status;
using grpcstatus = Grpc.Core.Status;
namespace Discount.Service.Helpers
{
    public static class GrpcErrorHelper
    {
        public static RpcException CreateValidationException(Dictionary<string, string> fieldErrors)
        {
            var fieldValidations = new List<Google.Rpc.BadRequest.Types.FieldViolation>();

            foreach (var error in fieldErrors)
            {
                fieldValidations.Add(new Google.Rpc.BadRequest.Types.FieldViolation
                {
                    Field = error.Key,
                    Description = error.Value
                });
            }
            var badrequest = new BadRequest();
            badrequest.FieldViolations.AddRange(fieldValidations);

            var status = new GoogleStatus
            {
                Code = (int)StatusCode.InvalidArgument,
                Message = "Validation Failed",
                Details = { Any.Pack(badrequest) }
            };

            var trailers = new Metadata {
                {"grpc-status-details-bin", status.ToByteArray() }
            };

            return new RpcException(new grpcstatus(StatusCode.InvalidArgument, "Validation Failed"), trailers);
        }
    }
}
