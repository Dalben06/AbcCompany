using AbcCompany.Orders.Domain.Entities;
using AbcCompany.Orders.Domain.Enums;

namespace AbcCompany.Orders.Domain.Tests
{
    public class OrderTests
    {
        [Fact]
        public void Constructor_Should_Initialize_With_Valid_Values()
        {
            // Arrange
            int clientId = 1;
            string clientName = "John Doe";
            int branchId = 1;
            string branchName = "Main Branch";
            int orderNumber = 123;
            List<OrderPayment> payments = new List<OrderPayment> {
                new OrderPayment(0,0,"",100)            };
            List<OrderProduct> products = new List<OrderProduct> { 
                new OrderProduct (0,0,"",110,1,10)
                };

            // Act
            var order = new Order(clientId, clientName, branchId, branchName, orderNumber, payments, products);

            // Assert
            Assert.Equal(orderNumber, order.OrderNumber);
            Assert.Equal(clientId, order.ClientId);
            Assert.Equal(clientName, order.ClientName);
            Assert.Equal(branchId, order.BranchId);
            Assert.Equal(branchName, order.BranchName);
            Assert.Equal(OrderStatus.Completed, order.OrderStatusId);
            Assert.Equal(nameof(OrderStatus.Completed), order.OrderStatusName);
            Assert.NotEmpty(order.Payments);
            Assert.NotEmpty(order.Products);
        }

        [Fact]
        public void SetOrderIdInPayments_Should_Set_OrderId_In_All_Payments()
        {
            // Arrange
            var orderId = 999;
            var order = new Order(1, "John Doe", 1, "Main Branch", 123, new List<OrderPayment>
            {
                new OrderPayment(),
                new OrderPayment()
            }, new List<OrderProduct>())
            { Id = orderId };
           
            // Act
            order.SetOrderIdInPayments();

            // Assert
            foreach (var payment in order.Payments)
                Assert.Equal(orderId, payment.OrderId);
        }

        [Fact]
        public void SetOrderIdInProducts_Should_Set_OrderId_In_All_Products()
        {
            var orderId = 999;
            // Arrange
            var order = new Order(1, "John Doe", 1, "Main Branch", 123, new List<OrderPayment>(), new List<OrderProduct>
            {
                new OrderProduct(),
                new OrderProduct()
            })
            {
                Id = orderId
            };
            
            // Act
            order.SetOrderIdInProducts();

            // Assert
            foreach (var product in order.Products)
                Assert.Equal(orderId, product.OrderId);
        }

        [Fact]
        public void Cancel_Should_Update_Status_And_Cancel_All_Products_And_Payments()
        {
            // Arrange
            var products = new List<OrderProduct>
        {
            new OrderProduct(),
            new OrderProduct()
        };
            var payments = new List<OrderPayment>
        {
            new OrderPayment(),
            new OrderPayment()
        };
            var order = new Order(1, "John Doe", 1, "Main Branch", 123, payments, products);

            // Act
            order.Cancel();

            // Assert
            Assert.Equal(OrderStatus.Canceled, order.OrderStatusId);
            Assert.Equal(nameof(OrderStatus.Canceled), order.OrderStatusName);
            foreach (var product in products)
                Assert.True(product.IsCanceled);
            foreach (var payment in payments)
                Assert.True(payment.IsCanceled);

        }

        [Fact]
        public void ValidatePaymentsAndProductHaveValueToCompleteOrder_Should_Return_True_If_Valid()
        {
            // Arrange
            var products = new List<OrderProduct>
            {
                new OrderProduct (0,0,"",110,1,10)
            };
            var payments = new List<OrderPayment>
            {
                new OrderPayment(0,0,"",100)
            };
            var order = new Order(1, "John Doe", 1, "Main Branch", 123, payments, products);

            // Act
            var result = order.ValidatePaymentsAndProductHaveValueToCompleteOrder();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ValidatePaymentsAndProductHaveValueToCompleteOrder_Should_Return_False_If_Invalid()
        {
            // Arrange
            var products = new List<OrderProduct>
            {
                new OrderProduct(0,0,"",1,110,0)
            };
            var payments = new List<OrderPayment>
            {
                new OrderPayment (0, 0, "", 50)
            };
            var order = new Order(1, "John Doe", 1, "Main Branch", 123, payments, products);

            // Act
            var result = order.ValidatePaymentsAndProductHaveValueToCompleteOrder();

            // Assert
            Assert.False(result);
        }
    }
}