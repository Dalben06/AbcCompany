using AbcCompany.Orders.Application.Models;
using FluentValidation;

namespace AbcCompany.Orders.Application.Commands.Validation
{
    public class NewOrderPaymentValidation : AbstractValidator<OrderPaymentModel>
    {
        public NewOrderPaymentValidation()
        {
            RuleFor(c => c.PaymentId)
               .GreaterThan(0)
               .WithMessage("Id do pagamento inválido");

            RuleFor(c => c.PaymentName)
              .NotEmpty()
              .WithMessage("O nome do pagamento não foi informado");

            RuleFor(c => c.Value)
                .GreaterThan(0)
                .WithMessage("valor do pagamento inválido");

        }
    }

    public class OrderPaymentValidation : AbstractValidator<OrderPaymentModel>
    {
        public OrderPaymentValidation()
        {
            RuleFor(c => c.Id)
             .GreaterThan(0)
             .WithMessage("Id do pagamento da venda inválido");

            RuleFor(c => c.PaymentId)
               .GreaterThan(0)
               .WithMessage("Id do pagamento inválido");

            RuleFor(c => c.PaymentName)
              .NotEmpty()
              .WithMessage("O nome do pagamento não foi informado");

            RuleFor(c => c.Value)
                .GreaterThan(0)
                .WithMessage("valor do pagamento inválido");

        }
    }
}
