namespace TodoApi.Infrastructure.Middleware
{
    internal static class ConfigureServices
    {
        internal static IServiceCollection AddExceptionMiddleware(this IServiceCollection services)
        {
            return services.AddScoped<ExceptionMiddleware>();
        }

        internal static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
        internal static IServiceCollection AddRequestLogging(this IServiceCollection services, IConfiguration config)
        {
            if (GetMiddlewareSettings(config).EnableHttpsLogging)
            {
                services.AddSingleton<RequestLoggingMiddleware>();
                services.AddScoped<ResponseLoggingMiddleware>();
            }

            return services;
        }
        internal static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, IConfiguration config)
        {
            if (GetMiddlewareSettings(config).EnableHttpsLogging)
            {
                app.UseMiddleware<RequestLoggingMiddleware>();
                app.UseMiddleware<ResponseLoggingMiddleware>();
            }

            return app;
        }
        private static MiddlewareSettings GetMiddlewareSettings(IConfiguration config)
        {
            var conf = config.GetSection(nameof(MiddlewareSettings)).Get<MiddlewareSettings>() ?? new MiddlewareSettings()
            {
                EnableHttpsLogging = true,
                EnableLocalization = false
            };

            return conf;
        }

    }
}