using AbcCompany.Core.Domain.Messages;

namespace AbcCompany.Orders.Application.Events
{
    public class OrderCanceledEvent : Event
    {
        public int Id { get; private set; }
        public int OrderNumber { get; private set; }
        public OrderCanceledEvent(int id, int orderNumber) {
            Id = id;
            OrderNumber = orderNumber;
        }
    }
}
