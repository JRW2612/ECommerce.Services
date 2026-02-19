namespace EventBus.Messages.Common
{
    public class EventBusConstant
    {
        public const string BasketCheckoutQueue = "basketcheckout-queue";
        public const string OrderCreatedQueue = "ordercreated-queue";
        public const string OrderUpdatedQueue = "orderupdated-queue";
        public const string OrderDeletedQueue = "orderdeleted-queue";
        public const string PaymentFailedQueue = "paymentfailed-queue";
        public const string PaymentSucceededQueue = "paymentsucceeded-queue";
    }
}
