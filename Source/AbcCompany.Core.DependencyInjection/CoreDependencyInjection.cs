using AbcCompany.Core.Domain.Configuration;
using AbcCompany.Core.Domain.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AbcCompany.Core.DependencyInjection
{
    public static class CoreDependencyInjection
    {

        public static void AddDependencyInjectionCoreModule(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddDomainServicesCore(configuration);
        }

        private static void AddDomainServicesCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<Settings>(x => configuration.Get<Settings>());

            services.AddScoped<DbContext>();
            services.AddScoped<DbFactory>();
        }

    }
}
