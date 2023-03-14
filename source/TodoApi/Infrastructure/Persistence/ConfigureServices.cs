using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Common.Persistence;
using TodoApi.Domain.Common;

namespace TodoApi.Infrastructure.Persistence
{
    internal static class ConfigureServices
    {

        internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("TodoApi");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("DB ConnectionString is not configured.");
            }

            services
                .AddDbContext<TodoDbContext>(opt => opt.UseSqlServer(connectionString))
                .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
                .AddRepositories();

            return services;
        }
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(TodoDbRepository<>));
            foreach (var aggregateRoot in typeof(IAggregateRoot).Assembly.GetExportedTypes().Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass).ToList())
            {
                services
                    .AddScoped(typeof(IReadRepository<>).MakeGenericType(aggregateRoot), sp => sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRoot)));
            }

            return services;
        }

    }
}
