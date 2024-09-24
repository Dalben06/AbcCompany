using FluentValidation;
using static AbcCompany.Orders.Application.Commands.RegisterNewOrderCommand;

namespace AbcCompany.Orders.Application.Commands
{
    public class CancelOrderCommand : OrderCommand
    {
        public CancelOrderCommand(int idOrder)
        {
            Id = idOrder;
        }

        public override bool IsValid()
        {
            ValidationResult = new CancelOrderCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }

    public class CancelOrderCommandValidation : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidation()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("Id da venda inválido");

        }
     
    }
}
