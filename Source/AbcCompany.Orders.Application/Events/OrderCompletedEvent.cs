using AbcCompany.Core.Domain.Messages;

namespace AbcCompany.Orders.Application.Events
{
    public class OrderCompletedEvent : Event
    {
        public int OrderNumber { get; private set; }
        public OrderCompletedEvent(int orderNumber) : base()
        {
            OrderNumber = orderNumber;
        }
    }
}
