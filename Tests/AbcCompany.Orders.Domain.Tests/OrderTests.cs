using AbcCompany.Orders.Domain.Entities;
using AbcCompany.Orders.Domain.Enums;
using AbcCompany.Orders.Domain.Tests.Mock;
using FluentAssertions;

namespace AbcCompany.Orders.Domain.Tests
{
    public class OrderTests
    {
        [Fact]
        public void Constructor_Should_Initialize_With_Valid_Values()
        {
            // Arrange
            
            List<OrderPayment> payments = new List<OrderPayment> {
                new OrderPayment(0,0,"",100)            };
            List<OrderProduct> products = new List<OrderProduct> { 
                new OrderProduct (0,0,"",110,1,10)
                };

            // Act
            var order =  OrderFake.GenerateFakeNewOrder();
            order.Payments.AddRange(payments);
            order.Products.AddRange(products);

            // Assert
            order.OrderStatusId.Should().Be(OrderStatus.Completed);
            order.OrderStatusName.Should().Be(nameof(OrderStatus.Completed));
            order.TotalProdutos.Should().Be(110);
            order.Total.Should().Be(100);
            order.Payments.Should().NotBeEmpty();
            order.Products.Should().NotBeEmpty();
        }

        [Fact]
        public void SetOrderIdInPayments_Should_Set_OrderId_In_All_Payments()
        {
            // Arrange
            var order = OrderFake.GenerateFakeNewOrder();
            order.Payments.Add(new OrderPayment());
            order.Payments.Add(new OrderPayment());
            // Act
            order.SetOrderIdInPayments();

            // Assert

            order.Payments.Should().HaveCount(2).And.OnlyContain(c => c.OrderId > 0);
            order.Products.Should().HaveCount(0);
        }

        [Fact]
        public void SetOrderIdInProducts_Should_Set_OrderId_In_All_Products()
        {

            // Arrange
            var order = OrderFake.GenerateFakeNewOrder();
            order.Products.Add(new OrderProduct());
            order.Products.Add(new OrderProduct());
            // Act
            order.SetOrderIdInProducts();

            // Assert

            order.Products.Should().HaveCount(2).And.OnlyContain(c => c.OrderId > 0);
            order.Payments.Should().HaveCount(0);
        }

        [Fact]
        public void Cancel_Should_Update_Status_And_Cancel_All_Products_And_Payments()
        {
            // Arrange
            var order = OrderFake.GenerateFakeNewOrder();
            order.Products.Add(new OrderProduct());
            order.Payments.Add(new OrderPayment());

            // Act
            order.Cancel();

            // Assert
            order.OrderStatusId.Should().Be(OrderStatus.Canceled);
            order.OrderStatusName.Should().Be(nameof(OrderStatus.Canceled));
            order.TotalProdutos.Should().Be(0);
            order.Total.Should().Be(0);
            order.Payments.Should().NotBeEmpty().And.NotContain(c => c.OrderPaymentStatusId == OrderPaymentStatus.Approved);
            order.Products.Should().NotBeEmpty().And.NotContain(c => c.OrderProductStatusId == OrderProductStatus.Active);
        }

        [Fact]
        public void ValidatePaymentsAndProductHaveValueToCompleteOrder_Should_Return_True_If_Valid()
        {
            // Arrange
            var order = OrderFake.GenerateFakeNewOrder();
            order.Products.Add(new OrderProduct(order.Id, 0, "", 110, 1, 10));
            order.Payments.Add(new OrderPayment(order.Id, 0, "", 100));
            // Act
            var result = order.ValidatePaymentsAndProductHaveValueToCompleteOrder();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ValidatePaymentsAndProductHaveValueToCompleteOrder_Should_Return_False_If_Invalid()
        {
            // Arrange

            var order = OrderFake.GenerateFakeNewOrder();
            order.Products.Add(new OrderProduct(order.Id, 0, "", 110, 1, 10));
            order.Payments.Add(new OrderPayment(order.Id, 0, "", 50));

            // Act
            var result = order.ValidatePaymentsAndProductHaveValueToCompleteOrder();

            // Assert
            result.Should().BeFalse();
        }
    }
}