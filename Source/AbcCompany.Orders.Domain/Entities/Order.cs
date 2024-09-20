using AbcCompany.Core.Domain.Entities;
using Dapper.Contrib.Extensions;

namespace AbcCompany.Orders.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order(int orderNumber, int clientId, string customerName, int branchId, string branchName, int orderStatusId, int orderStatusName, decimal total, decimal discountTotal)
        {
            OrderNumber = orderNumber;
            ClientId = clientId;
            CustomerName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            OrderStatusId = orderStatusId;
            OrderStatusName = orderStatusName;
            Total = total;
            DiscountTotal = discountTotal;
        }

        public int OrderNumber { get; private set; }
        public int ClientId { get; private set; }
        public string CustomerName { get; private set; }
        public int BranchId { get; private set; }
        public string BranchName { get; private set; }
        public int OrderStatusId { get; private set; }
        public int OrderStatusName { get; private set; }
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

        public void AddProduct(OrderPayment payment)
        {
            Payments.Add(payment);
        }

        public void AddPayments(List<OrderPayment> payments)
        {
            Payments.AddRange(payments);
        }

        public void AddProducts(List<OrderProduct> products)
        {
            Products.AddRange(products);
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
