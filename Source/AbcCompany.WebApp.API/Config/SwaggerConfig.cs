using Microsoft.OpenApi.Models;

namespace AbcCompany.WebApp.API.Config
{
    public static class SwaggerConfig
    {

        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AbcCompany Project",
                    Description = "AbcCompany API Swagger surface",
                    Contact = new OpenApiContact { Name = "Leonardo Dalben", Email = "contato@devdalben.com" },
                });



            });
        }

        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
    }
}
