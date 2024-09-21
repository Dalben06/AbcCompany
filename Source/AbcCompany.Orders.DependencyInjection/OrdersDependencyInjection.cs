using AbcCompany.Core.Domain.Entities;
using AbcCompany.Orders.Application.Commands;
using AbcCompany.Orders.Application.Events;
using AbcCompany.Orders.Application.Interfaces;
using AbcCompany.Orders.Application.Models;
using AbcCompany.Orders.Application.Services;
using AbcCompany.Orders.Data.Repositories;
using AbcCompany.Orders.Domain.IRepositories;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AbcCompany.Orders.DependencyInjection
{
    public static class OrdersDependencyInjection
    {
        public static void AddDependencyInjectionOrderModule(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddOrderRepositories();
            services.AddOrderCommands();
            services.AddOrderEvents();
            services.AddOrderServices();
        }

        private static void AddOrderServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
        }

        private static void AddOrderCommands(this IServiceCollection services)
        {
            services.AddScoped<IRequestHandler<RegisterNewOrderCommand, ResponseHttp<OrderModel>>, OrderCommandHandler>();
        }

        private static void AddOrderEvents(this IServiceCollection services)
        {
            services.AddScoped<INotificationHandler<OrderCompletedEvent>, OrderEventHandler>();
        }

        private static void AddOrderRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
        }

    }
}
