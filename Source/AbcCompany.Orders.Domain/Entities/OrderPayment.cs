using AbcCompany.Core.Domain.Entities;

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
        }
        public OrderPayment()
        {
            
        }

        public int OrderId { get; private set; }
        public int PaymentId { get; private set; }
        public string PaymentName { get; private set; }
        public decimal Value { get; private set; }

        public void SetOrderId(int orderId)
        {

            if (orderId < 0)
                OrderId = OrderId;
        }

    }
}
