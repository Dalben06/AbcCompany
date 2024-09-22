using AbcCompany.Orders.Application.Commands.Validation;
using AbcCompany.Orders.Application.Models;
using FluentValidation;

namespace AbcCompany.Orders.Application.Commands
{
    public class UpdateOrderAndProductsCommand : OrderCommand
    {
        public UpdateOrderAndProductsCommand(int idOrder,
            List<OrderProductModel> productsToCancel,
            List<OrderPaymentModel> paymentsToCancel,
            List<OrderProductModel> newProducts,
            List<OrderPaymentModel> newPayments)
        {
            
            ProductsToCancel = productsToCancel;
            PaymentsToCancel = paymentsToCancel;
            Products = newProducts;
            Payments = newPayments;
            Id = idOrder;
        }
        public override bool IsValid()
        {
            ValidationResult = new UpdateOrderAndProductsCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }

    public class UpdateOrderAndProductsCommandValidation : AbstractValidator<UpdateOrderAndProductsCommand>
    {
        public UpdateOrderAndProductsCommandValidation()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("Id da venda inválido");

            RuleFor(c => c.Total)
                .GreaterThan(0)
                .WithMessage("O total deve ser maior que 0");

            RuleForEach(c => c.Products).SetValidator(new NewOrderProductValidation());
            RuleForEach(c => c.Payments).SetValidator(new NewOrderPaymentValidation());

            RuleForEach(c => c.ProductsToCancel).SetValidator(new OrderProductValidation());
            RuleForEach(c => c.PaymentsToCancel).SetValidator(new OrderPaymentValidation());

        }
       
    }
}
