using AbcCompany.Core.Domain.Communication.Mediator;
using AbcCompany.Core.Domain.Messages;
using AbcCompany.Orders.Aplication.Tests.Mock;
using AbcCompany.Orders.Application.Commands;
using AbcCompany.Orders.Application.Events;
using AbcCompany.Orders.Application.Interfaces;
using AbcCompany.Orders.Application.Models;
using AbcCompany.Orders.Application.Services;
using AbcCompany.Orders.Domain.Entities;
using AbcCompany.Orders.Domain.IRepositories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcCompany.Orders.Aplication.Tests.Commands
{
    public class OrderCommandHandlerTests
    {

        private readonly OrderCommandHandler _handler;
        private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
        private readonly IOrderService _orderService = Substitute.For<IOrderService>();
        private readonly IMediatorHandler _mediatorHandler = Substitute.For<IMediatorHandler>();

        public OrderCommandHandlerTests()
        {
            _handler = new OrderCommandHandler(_orderService, _orderRepository,_mediatorHandler);
        }

        [Fact]
        public async Task RegisterNewOrder_ShouldReturnOrder()
        {
            // Arrange
            var orderNumberAndId = 1;
            var newOrderCommand = OrderCommandFake.GenerateNewOrderCommand();
            var products = new List<OrderProduct>();
            var payments = new List<OrderPayment>();
            var productsModel = new List<OrderProductModel>();
            var paymentsModel = new List<OrderPaymentModel>();
            foreach (var pay in newOrderCommand.Payments.Select((value, i) => (value, i)))
            {
                var payment = new OrderPayment(orderNumberAndId, pay.value.PaymentId, pay.value.PaymentName, pay.value.Value) { Id = pay.i };
                payments.Add(payment);
                paymentsModel.Add(new OrderPaymentModel() { 
                    Id = payment.Id,
                    PaymentId = payment.PaymentId,
                    PaymentName = payment.PaymentName,
                    Value = payment.Value,    
                });

            }

            foreach (var prod in newOrderCommand.Products.Select((value, i) => (value, i)))
            {
                var product = new OrderProduct(orderNumberAndId, prod.value.ProductId, prod.value.ProductName, prod.value.ProductUnitValue,
                    prod.value.Quantity, prod.value.Discount)
                { Id = prod.i };
                products.Add(product);

                productsModel.Add(new OrderProductModel()
                {
                    Id = product.Id,
                    Discount = product.Discount,
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductUnitValue = product.ProductUnitValue,
                    Quantity = product.Quantity,
                    Status = product.OrderProductStatusName,
                });
            }


            var order = new Order(
                newOrderCommand.ClientId,
                newOrderCommand.ClientName,
                newOrderCommand.BranchId,
                newOrderCommand.BranchName,
                orderNumberAndId, payments, products);
            _orderRepository.GetNextOrderNumber().Returns(orderNumberAndId);
            await _orderRepository.Add(order);
            _mediatorHandler.PublishEvent<OrderCompletedEvent>(new OrderCompletedEvent(orderNumberAndId)).Returns(Task.CompletedTask);
            _orderRepository.Get(order.Id,false).Returns(order);

            _orderService.GetById(order.Id, false).Returns(Task.FromResult(new Application.Models.OrderModel
            {
                Id = order.Id,
                BranchId = order.BranchId,
                BranchName = order.BranchName,
                ClientId = order.ClientId,
                ClientName = order.ClientName,
                Date = order.Date,
                DiscountTotal = order.DiscountTotal,
                OrderNumber = order.OrderNumber,
                OrderStatusId = (int)order.OrderStatusId,
                OrderStatusName = order.OrderStatusId.ToString(),
                Payments = paymentsModel,
                Products = productsModel,
            }));



            // Act
            var res = await _handler.Handle(newOrderCommand, new CancellationToken());

            // Assert
            res.Model.Should().NotBeNull();
            res.Validation.Errors.Should().BeEmpty();
            res.Model.Id.Should().Be(order.Id); 
            res.Model.OrderNumber.Should().Be(order.OrderNumber);
            res.Model.OrderStatusName.Should().Be(order.OrderStatusName);
            res.Model.BranchName.Should().Be(order.BranchName);
            res.Model.BranchId.Should().Be(order.BranchId);
            res.Model.ClientId.Should().Be(order.ClientId);
            res.Model.ClientName.Should().Be(order.ClientName);
            res.Model.Date.Should().Be(order.Date); 
            res.Model.DiscountTotal.Should().Be(order.DiscountTotal);
            res.Model.Payments.Should().HaveCount(paymentsModel.Count);
            res.Model.Products.Should().HaveCount(products.Count);
        }


    }
}
