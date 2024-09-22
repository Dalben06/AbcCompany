using AbcCompany.Core.DependencyInjection;
using AbcCompany.Orders.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbcCompany.DependencyInjection
{
    public static class DependencyInjectionServices
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDependencyInjectionCoreModule(configuration);
            services.AddDependencyInjectionOrderModule();



        }



    }

}
