using AbcCompany.Orders.Application.Commands;
using AbcCompany.Orders.Application.Models;
using Bogus;

namespace AbcCompany.Orders.Aplication.Tests.Mock
{
    public class OrderCommandFake
    {
        private static string[] paymentsName = ["Dinheiro", "Cartao Debito", "Cartao Credito", "Crediario", "Boleto", "Pix", "Voucher", "Apple Pay", "Link de pagamento"];
        public static RegisterNewOrderCommand GenerateNewOrderCommand()
        {
            
            var faker = new Faker("en");
            var totalOrder = faker.Random.Number(100);
            var payments = new List<OrderPaymentModel>();
            var products = new List<OrderProductModel>();
            int restToOrderPayment = totalOrder;
            int restToOrderProduct = totalOrder;
            while (restToOrderPayment > 0) {
                var paymentValue = faker.Random.Number(1,restToOrderPayment);
                var payment = new OrderPaymentModel()
                {
                    PaymentId = faker.Random.Number(100),
                    PaymentName = faker.PickRandom(paymentsName),
                    Value = paymentValue,
                };
                payments.Add(payment);
                restToOrderPayment -= paymentValue;
            }

            while (restToOrderProduct > 0)
            {
                var ProductValue = faker.Random.Number(1, restToOrderProduct);
                var product = new OrderProductModel()
                {
                    ProductId = faker.Random.Number(100),
                    ProductName = faker.Commerce.ProductName(),
                    ProductUnitValue = ProductValue,
                    Quantity = 1,
                };
                products.Add(product);
                restToOrderProduct -= ProductValue;
            }

            return new RegisterNewOrderCommand(faker.Random.Number(100), faker.Name.FullName(), faker.Random.Number(100), faker.Address.City(), payments, products);
        }
    }
}
