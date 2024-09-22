using AbcCompany.Core.Domain.Entities;
using AbcCompany.Orders.Domain.Enums;

namespace AbcCompany.Orders.Domain.Entities
{
    public class OrderPayment : BaseEntity
    {
        public OrderPayment(int orderId, int paymentId, string paymentName, decimal value)
        {
            OrderId = orderId;
            PaymentId = paymentId;
            PaymentName = paymentName;
            Value = value;
            OrderPaymentStatusId = OrderPaymentStatus.Approved;
            OrderPaymentStatusName = nameof(OrderPaymentStatus.Approved);
        }
        public OrderPayment()
        {
            
        }

        public int OrderId { get; private set; }
        public int PaymentId { get; private set; }
        public OrderPaymentStatus OrderPaymentStatusId { get; private set; }
        public string OrderPaymentStatusName { get; private set; }
        public string PaymentName { get; private set; }
        public decimal Value { get; private set; }
        public void CancelPayment()
        {
            OrderPaymentStatusId = OrderPaymentStatus.Canceled;
            OrderPaymentStatusName = nameof(OrderPaymentStatus.Canceled);
        }
        public void SetOrderId(int orderId)
        {

            if (orderId > 0)
                OrderId = orderId;
        }

    }
}
