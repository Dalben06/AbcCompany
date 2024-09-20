using AbcCompany.Orders.Data.Repositories;
using AbcCompany.Orders.Domain.IRepositories;
using Microsoft.Extensions.DependencyInjection;

namespace AbcCompany.Orders.DependencyInjection
{
    public static class OrdersDependencyInjection
    {
        public static void AddDependencyInjectionOrderModule(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddOrderRepositories();
        }

        private static void AddOrderRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
        }

    }
}
