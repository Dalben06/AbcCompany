﻿using AbcCompany.Orders.Application.Interfaces;
using AbcCompany.Orders.Application.Models;
using AbcCompany.Orders.Domain.Entities;
using AbcCompany.Orders.Domain.IRepositories;

namespace AbcCompany.Orders.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderModel>> GetAll()
        {
            var orders = await _orderRepository.GetAll();

            if (!orders.Any()) return Enumerable.Empty<OrderModel>();
            List<OrderModel> ordersModel = MapOrder(orders);

            return ordersModel;
        }

      
        public async Task<OrderModel> GetById(int id)
        {
            var order = await _orderRepository.Get(id);

            if (order == null) return null;

            return MapOrder(new List<Order>() { order})?.FirstOrDefault() ?? new OrderModel();
        }


        private List<OrderModel> MapOrder(IEnumerable<Order> orders)
        {
            var ordersModel = new List<OrderModel>();

            foreach (var order in orders)
            {
                var model = new OrderModel
                {
                    Id = order.Id,
                    Total = order.Total,
                    OrderStatusName = order.OrderStatusName,
                    BranchName = order.BranchName,
                    ClientName = order.ClientName,
                    DiscountTotal = order.DiscountTotal,
                    OrderNumber = order.OrderNumber,
                };
                var payments = new List<OrderPaymentModel>();
                foreach (var payment in order.Payments)
                    payments.Add(new OrderPaymentModel { Id = payment.Id, PaymentId = payment.PaymentId, PaymentName = payment.PaymentName, Value = payment.Value });

                var products = new List<OrderProductModel>();
                foreach (var product in order.Products)
                    products.Add(new OrderProductModel
                    {
                        Id = product.Id,
                        Discount = product.Discount,
                        ProductId = product.ProductId,
                        Quantity = product.Quantity,
                        ProductName = product.ProductName,
                        ProductUnitValue = product.ProductUnitValue
                    });

                model.Payments = payments;
                model.Products = products;
                ordersModel.Add(model);
            }

            return ordersModel;
        }

    }
}