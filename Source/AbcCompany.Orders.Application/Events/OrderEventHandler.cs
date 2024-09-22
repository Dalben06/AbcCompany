using MediatR;

namespace AbcCompany.Orders.Application.Events
{
    public class OrderEventHandler :
        INotificationHandler<OrderCompletedEvent>,
        INotificationHandler<OrderPaymentCanceledEvent>,
        INotificationHandler<OrderProductCanceledEvent>,
        INotificationHandler<OrderUpdatedEvent>,
        INotificationHandler<OrderCanceledEvent>
    {
        public Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
        {

            // Add message queue to send message

            // Send notification email the order has been completed!
            // 
            return Task.CompletedTask;
        }

        public Task Handle(OrderPaymentCanceledEvent notification, CancellationToken cancellationToken)
        {
            // Add message queue to send message 

            // Add Cash on CashFlow from company
            return Task.CompletedTask;
        }

        public Task Handle(OrderProductCanceledEvent notification, CancellationToken cancellationToken)
        {

            // add item on stock 

            return Task.CompletedTask;
        }

        public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            // Add message queue to send message

            // Send notification email the order has been updated!
            return Task.CompletedTask;
        }

        public Task Handle(OrderCanceledEvent notification, CancellationToken cancellationToken)
        {
            // Add message queue to send message

            // Send notification email the order has been canceled!
            return Task.CompletedTask;
        }
    }
}
