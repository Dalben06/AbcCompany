using AbcCompany.Core.Domain.Entities;
using AbcCompany.Orders.Domain.Enums;
using Dapper.Contrib.Extensions;

namespace AbcCompany.Orders.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order(int orderNumber, int clientId, string customerName, int branchId, string branchName, OrderStatus orderStatusId, decimal total, decimal discountTotal)
        {
            OrderNumber = orderNumber;
            ClientId = clientId;
            ClientName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            OrderStatusId = orderStatusId;
            OrderStatusName = orderStatusId.ToString();
            Total = total;
            DiscountTotal = discountTotal;
        }

        public Order(int clientId, string customerName, int branchId, string branchName, int orderNumber)
        {
            OrderNumber = orderNumber;
            ClientId = clientId;
            ClientName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            OrderStatusId = OrderStatus.Completed;
            OrderStatusName = nameof(OrderStatus.Completed);
        }

        public int OrderNumber { get; private set; }
        public int ClientId { get; private set; }
        public string ClientName { get; private set; }
        public int BranchId { get; private set; }
        public string BranchName { get; private set; }
        public OrderStatus OrderStatusId { get; private set; }
        public string OrderStatusName { get; private set; }
        public decimal Total { get; private set; }
        public decimal DiscountTotal { get; private set; }

        [Computed]
        public List<OrderPayment> Payments { get; private set; }

        [Computed]
        public List<OrderProduct> Products { get; private set; }

        public void AddPayment(OrderPayment payment) 
        {
            Payments.Add(payment);
        }

        public void AddProduct(OrderProduct product)
        {
            Products.Add(product);
            Total = Products.Sum(p => p.Total);
        }

        public void AddPayments(List<OrderPayment> payments)
        {
            Payments.AddRange(payments);
        }

        public void AddProducts(List<OrderProduct> products)
        {
            Products.AddRange(products);
            Total = Products.Sum(p => p.Total);
        }


        public void SetOrderIdInPayments()
        {
            if (!Payments.Any()) return;


            foreach (var payment in Payments)
                payment.SetOrderId(Id);
        }

        public void SetOrderIdInProducts()
        {
            if (!Products.Any()) return;

            foreach (var product in Products)
                product.SetOrderId(Id);
        }

    }
}
