using Ardalis.Specification;
using MediatR;
using TodoApi.Application.Common.Paginations;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Common.Specification;
using TodoApi.Domain.Todos;
using TodoApi.Dtos;

namespace TodoApi.Application.Todos
{
    public class SearchTodoRequest : PaginationFilter, IRequest<List<TodoDto>>
    {
        public TodoSearchCriteria SearchCriteria { get; set; }
        public SearchTodoRequest(TodoSearchCriteria searchCriteria)
        {
            SearchCriteria = searchCriteria;
        }
    }
    public class TodoBySearchSpec : EntitiesByPaginationFilterSpec<Todo, TodoDto>
    {
        public TodoBySearchSpec(SearchTodoRequest request) : base(request)
        {
            var search = request.SearchCriteria;
            Query.Include(x => x.TodoItems)
                .Where(x => (search.Id == null || x.Id == search.Id) &&
                    (search.Title == null || x.Title == search.Title) &&
                    (search.Description == null || x.Description == search.Description) &&
                    (search.Completed == null || x.Completed == search.Completed) &&
                    (search.CompletedOn == null || x.CompletedOn == search.CompletedOn) &&
                    (search.Progress == null || x.Progress == search.Progress));
        }
    }

    public class SearchTodoRequestHandler : IRequestHandler<SearchTodoRequest, List<TodoDto>>
    {
        private readonly IReadRepository<Todo> _repository;

        public SearchTodoRequestHandler(IReadRepository<Todo> repository) => _repository = repository;

        public async Task<List<TodoDto>> Handle(SearchTodoRequest request, CancellationToken cancellationToken)
        {
            var spec = new TodoBySearchSpec(request);
            return await _repository.ListAsync(spec, cancellationToken);
        }
    }

}
