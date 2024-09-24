using AbcCompany.Orders.Application.Interfaces;
using AbcCompany.Orders.Application.Services;
using AbcCompany.Orders.Domain.Entities;
using AbcCompany.Orders.Domain.IRepositories;
using AbcCompany.Orders.Domain.Tests.Mock;
using Bogus;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace AbcCompany.Orders.Aplication.Tests.Services
{
    public class OrderServiceTests
    {

        private readonly IOrderService _orderService;
        private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();

        public OrderServiceTests()
        {
            _orderService = new OrderService(_orderRepository);
        }
        [Fact]
        public async Task GetById_ShouldReturnOrder_WhenOrderExist() 
        {

            var order = OrderFake.GenerateFakeOrder(new Faker().Random.Number(100));

            _orderRepository.Get(order.Id).Returns(new Order(order.ClientId,order.ClientName, order.BranchId,
                order.BranchName,order.OrderNumber, new List<OrderPayment>(), new List<OrderProduct>()) {
                Id = order.Id
            });

            var res = await _orderService.GetById(order.Id);

            res.Should().NotBeNull();
            res.Id.Should().Be(order.Id);
            res.OrderNumber.Should().Be(order.OrderNumber);
        }


        [Fact]
        public async Task GetById_ShouldReturnOrder_WhenNotOrderExist()
        {

            var order = OrderFake.GenerateFakeOrder(0);

            _orderRepository.Get(order.Id).ReturnsNull();

            var res = await _orderService.GetById(order.Id);

            res.Should().BeNull();
        }

        [Fact]
        public async Task GetAll_ShouldReturnOrder_Registers()
        {
            int numberReturn = new Faker().Random.Number(100);
            var orders = OrderFake.GenerateFakeOrders(numberReturn);
            _orderRepository.GetAll().Returns(orders);

            var res = await _orderService.GetAll();

            res.Should().NotBeNull().And.NotBeEmpty().And.HaveCount(numberReturn);
        }

    }
}
