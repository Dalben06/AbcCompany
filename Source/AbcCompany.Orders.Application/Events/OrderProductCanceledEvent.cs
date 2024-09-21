using AbcCompany.Core.Domain.Messages;

namespace AbcCompany.Orders.Application.Events
{
    public class OrderProductCanceledEvent : Event
    {
        public OrderProductCanceledEvent(int id, int productId)
        {
            Id = id;
            ProductId = productId;
        }

        public int Id { get; private set; }
        public int ProductId { get; private set; }


    }
}
