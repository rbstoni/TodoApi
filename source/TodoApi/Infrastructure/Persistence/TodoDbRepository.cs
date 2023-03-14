using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Mapster;
using TodoApi.Application.Common.Persistence;
using TodoApi.Domain.Common;

namespace TodoApi.Infrastructure.Persistence
{
    public class TodoDbRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
    {
        public TodoDbRepository(TodoDbContext dbContext) : base(dbContext)
        {
        }
        protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
        {
            return ApplySpecification(specification, false).ProjectToType<TResult>();
        }
    }
}