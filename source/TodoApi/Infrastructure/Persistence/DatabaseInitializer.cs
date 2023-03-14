using Microsoft.EntityFrameworkCore;

namespace TodoApi.Infrastructure.Persistence
{
    internal class DatabaseInitializer : IDatabaseInitializer
    {

        private readonly ILogger<DatabaseInitializer> logger;
        private readonly TodoDbContext dbContext;

        public DatabaseInitializer(TodoDbContext dbContext, ILogger<DatabaseInitializer> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task InitializeDatabasesAsync(CancellationToken cancellationToken = default)
        {
            if (dbContext.Database.GetMigrations().Any())
            {
                if ((await dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    logger.LogInformation("Applying Migrations");
                    await dbContext.Database.MigrateAsync(cancellationToken);
                }

                if (await dbContext.Database.CanConnectAsync(cancellationToken))
                {
                    logger.LogInformation("Connection Database Succeeded.");
                }
            }
        }

    }
}