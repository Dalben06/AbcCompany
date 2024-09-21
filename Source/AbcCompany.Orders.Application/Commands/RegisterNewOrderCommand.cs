using AbcCompany.Orders.Application.Commands.Validation;
using AbcCompany.Orders.Application.Models;
using FluentValidation;

namespace AbcCompany.Orders.Application.Commands
{
    public class RegisterNewOrderCommand : OrderCommand
    {
        public RegisterNewOrderCommand(int clientId, string clientName, int branchId, string branchName
            , List<OrderPaymentModel> payments, List<OrderProductModel> products) : base()
        {
            ClientId = clientId;
            ClientName  = clientName;
            BranchId = branchId;
            BranchName = branchName;
            Payments = payments ?? new List<OrderPaymentModel>();
            Products = products ?? new List<OrderProductModel>();

        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterNewOrderCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }


        public class RegisterNewOrderCommandValidation : AbstractValidator<RegisterNewOrderCommand>
        {
            public RegisterNewOrderCommandValidation()
            {
                RuleFor(c => c.ClientId)
                    .GreaterThan(0)
                    .WithMessage("Id do cliente inválido");

                RuleFor(c => c.BranchId)
                    .GreaterThan(0)
                    .WithMessage("Id do Filial inválido");

                RuleFor(c => c.ClientName)
                    .NotEmpty()
                    .WithMessage("O nome do cliente não foi informado");

                RuleFor(c => c.Total)
                    .GreaterThan(0)
                    .WithMessage("O total deve ser maior que 0");

                RuleForEach(c => c.Products).SetValidator(new NewOrderProductValidation());
                RuleForEach(c => c.Payments).SetValidator(new NewOrderPaymentValidation());
                RuleFor(c => c)
                   .Must(HaveEqualPaymentsAndProductsTotal)
                   .WithMessage("O total dos pagamentos deve ser igual ao total dos produtos.");

            }
            public static bool HaveEqualPaymentsAndProductsTotal(RegisterNewOrderCommand command)
            {
                var totalPayments = command.Payments.Sum(p => p.Value);
                return totalPayments == command.Total;
            }
        }

  

      
    }
}
