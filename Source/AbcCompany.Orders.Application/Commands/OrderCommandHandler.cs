using AbcCompany.Core.Domain.Communication.Mediator;
using AbcCompany.Core.Domain.Messages;
using AbcCompany.Orders.Application.Events;
using AbcCompany.Orders.Domain.Entities;
using AbcCompany.Orders.Domain.IRepositories;
using FluentValidation.Results;
using MediatR;
using System.Transactions;

namespace AbcCompany.Orders.Application.Commands
{
    public class OrderCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewOrderCommand, ValidationResult>
    {
        private readonly IOrderRepository _orderRepository;

        public OrderCommandHandler(IOrderRepository orderRepository, IMediatorHandler mediatorHandler): base(mediatorHandler)
        {
            _orderRepository = orderRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<ValidationResult> Handle(RegisterNewOrderCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            try
            {
                var order = new Order(message.ClientId, message.ClientName, message.BranchId, message.BranchName, await _orderRepository.GetNextOrderNumber());
                var products = new List<OrderProduct>();
                foreach (var product in message.Products)
                    products.Add(new OrderProduct(0, product.ProductId, product.ProductName, product.ProductUnitValue, product.Quantity, product.Discount));

                var payments = new List<OrderPayment>();
                foreach (var payment in message.Payments)
                    payments.Add(new OrderPayment(0, payment.PaymentId, payment.PaymentName, payment.Value));

                order.AddPayments(payments);
                order.AddProducts(products);
                order.AddDomainEvent(new OrderCompletedEvent(order.OrderNumber));
                AddDomainChanges(order);
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _orderRepository.Add(order);
                    scope.Complete();
                }
                await Commit();
            }
            catch (Exception e)
            {
                AddError(e.Message);
            }

            return message.ValidationResult;
        }
    }
}
