namespace AbcCompany.Orders.Application.Models
{
    public class OrderProductModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductUnitValue { get; set; }
        public decimal Quantity { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public decimal Total => (Quantity * ProductUnitValue) - Discount;
    }
}
