namespace AbcCompany.Orders.Application.Models
{
    public class ChangeOrderModel
    {
        public int Id { get; set; }
        public List<OrderPaymentModel> Payments { get; set; }
        public List<OrderPaymentModel> PaymentsToCancel { get; set; }
        public List<OrderProductModel> Products { get; set; }
        public List<OrderProductModel> ProductsToCancel { get; set; }
    }
}
