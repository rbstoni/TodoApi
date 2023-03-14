using Ardalis.Specification;
using TodoApi.Application.Common.Paginations;

namespace TodoApi.Application.Common.Specification
{
    public class EntitiesByBaseFilterSpec<T, TResult> : Specification<T, TResult>
    {

        public EntitiesByBaseFilterSpec(BaseFilter filter) => Query.SearchBy(filter);

    }
    public class EntitiesByBaseFilterSpec<T> : Specification<T>
    {

        public EntitiesByBaseFilterSpec(BaseFilter filter) => Query.SearchBy(filter);

    }
}