using FluentValidation;
using System.Reflection;
using TodoApi.Infrastructure.Common;
using TodoApi.Infrastructure.Mapping;
using TodoApi.Infrastructure.Middleware;
using TodoApi.Infrastructure.Persistence;
using TodoApi.Infrastructure.SecurityHeaders;

namespace TodoApi.Infrastructure
{
    public static class ConfigureServices
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            MapsterSettings.Configure();
            return services
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
                .AddPersistence(config)
                .AddExceptionMiddleware()
                .AddRequestLogging(config)
                .AddServices();
        }
        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config)
        {
            return builder
                .UseSecurityHeaders(config)
                .UseExceptionMiddleware()
                .UseRequestLogging(config);
        }

    }
}
