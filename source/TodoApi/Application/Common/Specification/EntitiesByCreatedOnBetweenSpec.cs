using Ardalis.Specification;
using TodoApi.Domain.Common;

namespace TodoApi.Application.Common.Specification
{
    public class EntitiesByCreatedOnBetweenSpec<T> : Specification<T> where T : BaseEntity
    {

        public EntitiesByCreatedOnBetweenSpec(DateTime from, DateTime until)
        {
            Query.Where(e => e.CreatedOn >= from && e.CreatedOn <= until);
        }

    }
}