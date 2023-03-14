namespace TodoApi.Infrastructure.Persistence
{
    internal interface IDatabaseInitializer
    {

        Task InitializeDatabasesAsync(CancellationToken cancellationToken = default);

    }
}