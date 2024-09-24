using AbcCompany.Orders.Domain.Entities;
using AbcCompany.Orders.Domain.Enums;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbcCompany.Orders.Domain.Tests.Mock
{
    public class OrderFake
    {

        public static List<Order> GenerateFakeOrders(int numOrders)
        {
            var id = 1;
            var fakerData = new Faker("en");

            var faker = new Faker<Order>()
                .RuleFor(o => o.Id, f => id++)
                .RuleFor(o => o.OrderNumber, f => id)
                .RuleFor(o => o.Date, f => fakerData.Date.Past())
                .RuleFor(o => o.BranchName, f => fakerData.Address.City())
                .RuleFor(o => o.ClientName, f => fakerData.Name.FullName())
                .RuleFor(o => o.Date, f => fakerData.Date.Past())
                .RuleFor(o => o.ClientId, f => fakerData.Random.Number(1, 1000))
                .RuleFor(o => o.BranchId, f => fakerData.Random.Number(1, 1000))
                .RuleFor(o => o.OrderStatusId, f => (OrderStatus)fakerData.Random.Number(1, 2));


            return faker.Generate(numOrders);
        }

        public static Order GenerateFakeOrder(int id)
        {
            var fakerData = new Faker("en");

            return new Order(fakerData.Random.Number(1, 1000), fakerData.Name.FullName(), fakerData.Random.Number(1, 1000), fakerData.Address.FullAddress(),
                id, null, null)
            { Id = id };
        }

        public static Order GenerateFakeNewOrder()
        {
            var id = 1;
            var fakerData = new Faker("en");

            return new Order(fakerData.Random.Number(1, 1000), fakerData.Name.FullName(), fakerData.Random.Number(1, 1000), fakerData.Address.FullAddress(),
                id, null, null)
            { Id = id };
        }
    }
}
