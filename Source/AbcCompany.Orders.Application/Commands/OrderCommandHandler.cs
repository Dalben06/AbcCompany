using AbcCompany.Core.Domain.Communication.Mediator;
using AbcCompany.Core.Domain.Entities;
using AbcCompany.Core.Domain.Messages;
using AbcCompany.Orders.Application.Events;
using AbcCompany.Orders.Application.Interfaces;
using AbcCompany.Orders.Application.Models;
using AbcCompany.Orders.Domain.Entities;
using AbcCompany.Orders.Domain.IRepositories;
using FluentValidation.Results;
using MediatR;
using System.Transactions;

namespace AbcCompany.Orders.Application.Commands
{
    public class OrderCommandHandler : CommandHandler,
        IRequestHandler<RegisterNewOrderCommand, ResponseHttp<OrderModel>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderService _orderService;

        public OrderCommandHandler(IOrderService orderService, IOrderRepository orderRepository, IMediatorHandler mediatorHandler): base(mediatorHandler)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<ResponseHttp<OrderModel>> Handle(RegisterNewOrderCommand message, CancellationToken cancellationToken)
        {
            var res =  new ResponseHttp<OrderModel>();
            if (!message.IsValid())
            {
                res.Validation.Errors.AddRange(message.ValidationResult.Errors);
                return res;
            }

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
                using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _orderRepository.Add(order);
                    scope.Complete();
                }
                await Commit();
                res.Model = await _orderService.GetById(order.Id);
            }
            catch (Exception e)
            {
                res.Validation.Errors.Add(new ValidationFailure(string.Empty, e.Message));
            }

            return res;
        }
    }
}
