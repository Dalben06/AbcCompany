using AbcCompany.Core.Domain.Communication.Mediator;
using AbcCompany.Core.Domain.Configuration;
using AbcCompany.Core.Domain.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(x => x.FullName.ToLower().Contains("abccompany")).ToArray();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));
            services.AddScoped<IMediatorHandler, MediatorHandler>();
        }

    }
}
