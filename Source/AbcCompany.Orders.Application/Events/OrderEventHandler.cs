using MediatR;
using Microsoft.Extensions.Logging;

namespace AbcCompany.Orders.Application.Events
{
    public class OrderEventHandler :
        INotificationHandler<OrderCompletedEvent>,
        INotificationHandler<OrderPaymentCanceledEvent>,
        INotificationHandler<OrderProductCanceledEvent>,
        INotificationHandler<OrderUpdatedEvent>,
        INotificationHandler<OrderCanceledEvent>
    {
        private readonly ILogger<OrderEventHandler> _logger;
        public OrderEventHandler(ILogger<OrderEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
        {

            // Add message queue to send message

            // Send notification email the order has been completed!
            // 

            _logger.LogInformation("Order Completed Event Completed!");
            return Task.CompletedTask;
        }

        public Task Handle(OrderPaymentCanceledEvent notification, CancellationToken cancellationToken)
        {
            // Add message queue to send message 

            // Add Cash on CashFlow from company
            _logger.LogInformation("Order Payment Canceled Completed!");

            return Task.CompletedTask;
        }

        public Task Handle(OrderProductCanceledEvent notification, CancellationToken cancellationToken)
        {

            // add item on stock 
            _logger.LogInformation("Order Product Canceled Completed!");
            return Task.CompletedTask;
        }

        public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
        {
            // Add message queue to send message

            // Send notification email the order has been updated!
            _logger.LogInformation("Order Update Completed!");
            return Task.CompletedTask;
        }

        public Task Handle(OrderCanceledEvent notification, CancellationToken cancellationToken)
        {
            // Add message queue to send message

            // Send notification email the order has been canceled!
            _logger.LogInformation("Order Canceled Completed!");
            return Task.CompletedTask;
        }
    }
}
