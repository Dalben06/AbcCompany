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
        IRequestHandler<RegisterNewOrderCommand, ResponseHttp<OrderModel>>,
        IRequestHandler<UpdateOrderAndProductsCommand, ResponseHttp<OrderModel>>,
        IRequestHandler<CancelOrderCommand, ResponseHttp<OrderModel>>


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
                res.AddErrors(message.ValidationResult);
                return res;
            }

            try
            {
                var products = new List<OrderProduct>();
                foreach (var product in message.Products)
                    products.Add(new OrderProduct(0, product.ProductId, product.ProductName, product.ProductUnitValue, product.Quantity, product.Discount));

                var payments = new List<OrderPayment>();
                foreach (var payment in message.Payments)
                    payments.Add(new OrderPayment(0, payment.PaymentId, payment.PaymentName, payment.Value));

                var order = new Order(message.ClientId, message.ClientName, message.BranchId, message.BranchName
                    , await _orderRepository.GetNextOrderNumber(), payments, products );
               
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

        public async Task<ResponseHttp<OrderModel>> Handle(UpdateOrderAndProductsCommand message, CancellationToken cancellationToken)
        {
            var res = new ResponseHttp<OrderModel>();
            if (!message.IsValid())
            {
                res.AddErrors(message.ValidationResult);
                return res;
            }

            try
            {
                var order = await _orderRepository.Get(message.Id);
                if (order == null) throw new Exception("Venda nao encontrada!");

                if (message.PaymentsToCancel.Any())
                {
                    foreach (var payToCancel in message.PaymentsToCancel)
                    {
                        var payment = order.Payments.FirstOrDefault(c => c.Id == payToCancel.Id);
                        if (payment == null) {
                            res.AddError($"Nao foi possivel encontrar o pagamento para cancelar : {payToCancel.PaymentName} - {payToCancel.Id}");
                            continue;
                        }

                        payment.CancelPayment();
                        order.AddDomainEvent(new OrderPaymentCanceledEvent(payToCancel.Id, payToCancel.PaymentId));
                    }
                }

                if (message.ProductsToCancel.Any())
                {
                    foreach (var prodToCancel in message.ProductsToCancel)
                    {
                        var product = order.Products.FirstOrDefault(c => c.Id == prodToCancel.Id);
                        if (product == null)
                        {
                            res.AddError($"Nao foi possivel encontrar o produto para cancelar : {prodToCancel.ProductName} - {prodToCancel.Id}");
                            continue;
                        }

                        product.CancelProduct();
                        order.AddDomainEvent(new OrderProductCanceledEvent(prodToCancel.Id, prodToCancel.ProductId));
                    }
                }

                var products = new List<OrderProduct>();
                foreach (var product in message.Products)
                    order.Products.Add(new OrderProduct(order.Id, product.ProductId, product.ProductName, product.ProductUnitValue, product.Quantity, product.Discount));

                var payments = new List<OrderPayment>();
                foreach (var payment in message.Payments)
                    order.Payments.Add(new OrderPayment(order.Id, payment.PaymentId, payment.PaymentName, payment.Value));

                if (!order.ValidatePaymentsAndProductHaveValueToCompleteOrder())
                    res.AddError("Os de pagamento e produtos não batem, favor revisar!");

                if (!res.Validation.IsValid) return res;

                order.AddDomainEvent(new OrderUpdatedEvent(order.Id,order.OrderNumber));
                AddDomainChanges(order);

                using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _orderRepository.Update(order);
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

        public async Task<ResponseHttp<OrderModel>> Handle(CancelOrderCommand message, CancellationToken cancellationToken)
        {
            var res = new ResponseHttp<OrderModel>();
            if (!message.IsValid())
            {
                res.AddErrors(message.ValidationResult) ;
                return res;
            }

            try
            {
                var order = await _orderRepository.Get(message.Id);
                if (order == null) throw new Exception("Venda nao encontrada!");

                order.Cancel();
                order.AddDomainEvent(new OrderCanceledEvent(order.Id, order.OrderNumber));
                order.Products.ForEach(p => { order.AddDomainEvent(new OrderProductCanceledEvent(p.Id, p.ProductId)); });
                order.Payments.ForEach(p => { order.AddDomainEvent(new OrderPaymentCanceledEvent(p.Id, p.PaymentId)); });
                AddDomainChanges(order);
                using (var scope = new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _orderRepository.Cancel(order);
                    scope.Complete();
                }
                await Commit();
                res.Model = await _orderService.GetById(order.Id, true);
            }
            catch (Exception e)
            {
                res.Validation.Errors.Add(new ValidationFailure(string.Empty, e.Message));
            }

            return res;
        }
    }
}
