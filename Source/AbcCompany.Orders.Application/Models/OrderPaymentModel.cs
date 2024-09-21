namespace AbcCompany.Orders.Application.Models
{
    public class OrderPaymentModel
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public string PaymentName { get; set; }
        public decimal Value { get; set; }
    }
}
