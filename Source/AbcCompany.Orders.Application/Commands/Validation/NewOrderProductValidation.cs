using AbcCompany.Orders.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcCompany.Orders.Application.Commands.Validation
{
    public class NewOrderProductValidation : AbstractValidator<OrderProductModel>
    {
        public NewOrderProductValidation()
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

    public class OrderProductValidation : AbstractValidator<OrderProductModel>
    {
        public OrderProductValidation()
        {
            RuleFor(c => c.Id)
               .GreaterThan(0)
               .WithMessage("Id do produto venda inválido");

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
}
