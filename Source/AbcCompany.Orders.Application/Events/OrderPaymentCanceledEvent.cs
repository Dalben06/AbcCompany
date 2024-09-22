using AbcCompany.Core.Domain.Messages;

namespace AbcCompany.Orders.Application.Events
{
    public class OrderPaymentCanceledEvent : Event
    {
        public OrderPaymentCanceledEvent(int id, int paymentId): base()
        {
            Id = id;
            PaymentId = paymentId;
        }

        public int Id { get; private set; }
        public int PaymentId { get; private set; }
    }
}
