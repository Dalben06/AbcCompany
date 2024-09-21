using AbcCompany.Core.Domain.Messages;
using AbcCompany.Orders.Application.Models;

namespace AbcCompany.Orders.Application.Commands
{
    public abstract class OrderCommand : Command
    {
        public int OrderNumber { get; protected set; }
        public int ClientId { get; protected set; }
        public string ClientName { get; protected set; }
        public int BranchId { get; protected set; }
        public string BranchName { get; protected set; }
        public int OrderStatusId { get; protected set; }
        public int OrderStatusName { get; protected set; }
        public decimal Total { get; protected set; }
        public decimal DiscountTotal { get; protected set; }
        public List<OrderPaymentModel> Payments { get; protected set; }
        public List<OrderProductModel> Products { get; protected set; }

        protected OrderCommand()
        {
            Payments = Payments ?? new List<OrderPaymentModel>();
            Products = Products ?? new List<OrderProductModel>();
        }
    }
}
