using Microsoft.AspNetCore.Identity;
using WebToApp2.Services;
using WebToApp2.Services.File;

namespace WebToApp2
{
    public static class ApplicationDependencyInjection
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddServices();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(typeof(UrlGenerator));
            services.AddScoped<IGeneratorService, GeneratorManager>();
            services.AddScoped<IAuthService, AuthManager>();
            services.AddScoped<IFileService, FileManager>();
        }
    }
}
