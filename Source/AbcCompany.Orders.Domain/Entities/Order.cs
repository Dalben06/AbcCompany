using AbcCompany.Core.Domain.Entities;
using AbcCompany.Orders.Domain.Enums;
using Dapper.Contrib.Extensions;

namespace AbcCompany.Orders.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order(int clientId, string customerName, int branchId, string branchName, int orderNumber
            , List<OrderPayment> payments, List<OrderProduct> products)
        {
            OrderNumber = orderNumber;
            ClientId = clientId;
            ClientName = customerName;
            BranchId = branchId;
            BranchName = branchName;
            OrderStatusId = OrderStatus.Completed;
            OrderStatusName = nameof(OrderStatus.Completed);
            Date = DateTime.Now;
            Payments = payments ?? new List<OrderPayment>();
            Products = products ?? new List<OrderProduct>();
        }
        // Dapper
        public Order()
        {
            Payments = new List<OrderPayment>();
            Products = new List<OrderProduct>();
        }
        public int OrderNumber { get; private set; }
        public int ClientId { get; private set; }
        public string ClientName { get; private set; }
        public int BranchId { get; private set; }
        public string BranchName { get; private set; }
        public OrderStatus OrderStatusId { get; private set; }
        public string OrderStatusName { get; private set; }
        public DateTime Date { get; private set; }

        [Computed]  
        public decimal TotalProdutos => Products.Where(p => p.OrderProductStatusId == OrderProductStatus.Active).Sum(p => p.Total);
        public decimal Total => TotalProdutos - DiscountTotal;
        public decimal DiscountTotal => Products.Where(p => p.OrderProductStatusId == OrderProductStatus.Active).Sum(p => p.Discount);

        [Computed]
        public List<OrderPayment> Payments { get; private set; }

        [Computed]
        public List<OrderProduct> Products { get; private set; }


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

        public bool ValidatePaymentsAndProductHaveValueToCompleteOrder()
        {
            if (TotalProdutos != Payments.Where(p => p.OrderPaymentStatusId == OrderPaymentStatus.Approved).Sum(p => p.Value))
                return false;

            return true;
        }
        public void Cancel()
        {
            OrderStatusId = OrderStatus.Canceled;
            OrderStatusName = nameof(OrderStatus.Canceled);
            foreach (var product in Products)
                product.CancelProduct();

            foreach (var pay in Payments)
                pay.CancelPayment();
        }

    }
}
