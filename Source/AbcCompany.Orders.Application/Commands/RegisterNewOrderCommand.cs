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
            CalculateTotal();
            CalculateDiscount();

        }

        private void CalculateTotal()
        {
            Total = Products.Sum(p => p.Total);
        }

        private void CalculateDiscount()
        {
            DiscountTotal = Products.Sum(p => p.Discount);
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

                RuleForEach(c => c.Products).SetValidator(new RegisterNewOrderCommandProdutsValidation());
                RuleForEach(c => c.Payments).SetValidator(new RegisterNewOrderCommandPaymentsValidation());
                RuleFor(c => c)
                   .Must(HaveEqualPaymentsAndProductsTotal)
                   .WithMessage("O total dos pagamentos deve ser igual ao total dos produtos.");

            }
            private bool HaveEqualPaymentsAndProductsTotal(RegisterNewOrderCommand command)
            {
                var totalPayments = command.Payments.Sum(p => p.Value);
                return totalPayments == command.Total;
            }
        }

        public class RegisterNewOrderCommandProdutsValidation : AbstractValidator<OrderProductModel> 
        {
            public RegisterNewOrderCommandProdutsValidation()
            {
                RuleFor(c => c.ProductId)
                   .GreaterThan(0)
                   .WithMessage("Id do produto inválido");

                RuleFor(c => c.ProductName)
                  .NotEmpty()
                  .WithMessage("O nome do produto não foi informado");

                RuleFor(c => c.Quantity)
                    .GreaterThan(0)
                    .WithMessage("quantidade do produto inválido");

                RuleFor(c => c.ProductUnitValue)
                  .GreaterThan(0)
                  .WithMessage("O valor da unidade do produto inválido");

                RuleFor(c => c.Total)
                  .GreaterThan(0)
                  .WithMessage("total do produto inválido");
            }

        }

        public class RegisterNewOrderCommandPaymentsValidation : AbstractValidator<OrderPaymentModel>
        {
            public RegisterNewOrderCommandPaymentsValidation()
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
    }
}
