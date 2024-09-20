using AbcCompany.Core.Domain.Entities;

namespace AbcCompany.Orders.Domain.Entities
{
    public class OrderProduct : BaseEntity
    {
        public OrderProduct(int orderId, int productId, string productName, decimal productUnitValue, decimal productQuantity, decimal discount, decimal total)
        {
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            ProductUnitValue = productUnitValue;
            ProductQuantity = productQuantity;
            Discount = discount;
            Total = total;
        }
        public OrderProduct()
        {
            
        }
        public int OrderId { get; private set; }
        public int ProductId { get; private set; }
        public string ProductName { get; private set; }
        public decimal ProductUnitValue { get; private set; }
        public decimal ProductQuantity { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Total { get; private set; }

        public void SetOrderId(int orderId) {

            if (orderId < 0)
                OrderId = OrderId;
        }
    }
}
