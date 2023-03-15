using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Common.Exceptions;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Common.Result;
using TodoApi.Application.Common.Validation;
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
    public class TodoItemSearchRequestValidator : CustomValidator<TodoItemSearchRequest>
    {
        public TodoItemSearchRequestValidator(IReadRepository<Todo> repository)
        {
            RuleFor(x => x.TodoId)
                .NotEqual(0)
                .MustAsync(async (id, ct) => await repository.AnyAsync(new TodoByIdSpec(id), ct) == true);
        }
    }
    public class TodoItemSearchRequestHandler : IRequestHandler<TodoItemSearchRequest, IEnumerable<TodoItemDto>>
    {
        private readonly TodoItemSearchRequestValidator validator;
        private readonly IReadRepository<Todo> repository;

        public TodoItemSearchRequestHandler(TodoItemSearchRequestValidator validator, IReadRepository<Todo> repository)
        {
            this.validator = validator;
            this.repository = repository;
        }

        public async Task<IEnumerable<TodoItemDto>> Handle(TodoItemSearchRequest request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var todo = await repository.FirstOrDefaultAsync(new TodoByIdIncludeTodoItemSpec(request.TodoId), cancellationToken);
            if (todo != null)
            {
                var todoItems = todo.TodoItems.AsQueryable();
                if (request.SearchCriteria.Done != null)
                {
                    todoItems = todoItems?.Where(x => x.Done == request.SearchCriteria.Done);
                }

                if (request.SearchCriteria.Id != null)
                {
                    todoItems = todoItems?.Where(x => x.Id == request.SearchCriteria.Id);
                }

                if (!string.IsNullOrEmpty(request.SearchCriteria.Title))
                {
                    todoItems = todoItems?.Where(x => x.Title == request.SearchCriteria.Title);
                }
                var result = todoItems?.Select(x => new TodoItemDto(x)).ToArray();
                return result!.ToList();
            }
            throw new NotFoundException("Todo not found");
        }
    }
}
