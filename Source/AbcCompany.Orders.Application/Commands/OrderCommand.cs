using AbcCompany.Core.Domain.Messages;
using AbcCompany.Orders.Application.Models;

namespace AbcCompany.Orders.Application.Commands
{
    public abstract class OrderCommand : Command<OrderModel> 
    {
        public int Id { get; protected set; }
        public int OrderNumber { get; protected set; }
        public int ClientId { get; protected set; }
        public string ClientName { get; protected set; }
        public int BranchId { get; protected set; }
        public string BranchName { get; protected set; }
        public int OrderStatusId { get; protected set; }
        public int OrderStatusName { get; protected set; }
        public decimal Total  => Products.Sum(p => p.Total);
        public decimal DiscountTotal => Products.Sum(p => p.Discount);
        public List<OrderPaymentModel> Payments { get; protected set; }
        public List<OrderPaymentModel> PaymentsToCancel { get; protected set; }
        public List<OrderProductModel> Products { get; protected set; }
        public List<OrderProductModel> ProductsToCancel { get; protected set; }

        protected OrderCommand()
        {
            Payments = Payments ?? new List<OrderPaymentModel>();
            PaymentsToCancel = PaymentsToCancel ?? new List<OrderPaymentModel>();
            Products = Products ?? new List<OrderProductModel>();
            ProductsToCancel = ProductsToCancel ?? new List<OrderProductModel>();
        }
    }
}
