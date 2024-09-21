using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcCompany.Orders.Application.Events
{
    public class OrderEventHandler :
        INotificationHandler<OrderCompletedEvent>
    {
        public Task Handle(OrderCompletedEvent notification, CancellationToken cancellationToken)
        {

            // Add message queue to send message
            return Task.CompletedTask;
        }
    }
}
