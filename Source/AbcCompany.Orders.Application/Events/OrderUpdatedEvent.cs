using AbcCompany.Core.Domain.Messages;

namespace AbcCompany.Orders.Application.Events
{
    public class OrderUpdatedEvent : Event
    {
        public OrderUpdatedEvent(int id, int orderNumber) : base()
        {
            Id = id;
            OrderNumber = orderNumber;
        }

        public int Id { get; private set; }
        public int OrderNumber { get; private set; }

    }
}
