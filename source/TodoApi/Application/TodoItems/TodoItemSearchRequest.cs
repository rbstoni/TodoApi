using MediatR;
using System.Collections.Immutable;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Todos;
using TodoApi.Domain.Todos;

namespace TodoApi.Dtos
{
    public class TodoItemSearchRequest : IRequest<IEnumerable<TodoItemDto>>
    {
        public int TodoId { get; set; }
        public TodoItemSearchCriteria SearchCriteria { get; set; }
        public TodoItemSearchRequest(int todoId, TodoItemSearchCriteria searchCriteria)
        {
            TodoId = todoId;
            SearchCriteria = searchCriteria;
        }
    }
    public class TodoItemSearchRequestHandler : IRequestHandler<TodoItemSearchRequest, IEnumerable<TodoItemDto>>
    {
        private readonly IReadRepository<Todo> repository;

        public TodoItemSearchRequestHandler(IReadRepository<Todo> repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<TodoItemDto>> Handle(TodoItemSearchRequest request, CancellationToken cancellationToken)
        {
            var todo = await repository.FirstOrDefaultAsync(new TodoByIdSpec(request.TodoId));
            var todoItems = todo?.TodoItems.AsQueryable();
            if (request.SearchCriteria.Done != null)
            {
                todoItems = todoItems?.Where(x => x.Done == request.SearchCriteria.Done);
            }

            if (request.SearchCriteria.Id != null)
            {
                todoItems = todoItems?.Where(x => x.Id == request.SearchCriteria.Id);
            }

            if (!string.IsNullOrEmpty(request.SearchCriteria.Name))
            {
                todoItems = todoItems?.Where(x => x.Title == request.SearchCriteria.Name);
            }

            var result = todoItems?.Select(x => new TodoItemDto(x)).ToImmutableArray() ?? new ImmutableArray<TodoItemDto>();

            return result;
        }
    }
}
