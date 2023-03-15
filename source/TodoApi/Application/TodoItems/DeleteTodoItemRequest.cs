using FluentValidation;
using MediatR;
using TodoApi.Application.Common.Exceptions;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Common.Validation;
using TodoApi.Application.Todos;
using TodoApi.Domain.Todos;

namespace TodoApi.Application.TodoItems
{
    public class DeleteTodoItemRequest : IRequest<int>
    {
        public int TodoId { get; set; }
        public int TodoItemId { get; set; }

        public DeleteTodoItemRequest(int todoId, int todoItemId)
        {
            TodoId = todoId;
            TodoItemId = todoItemId;
        }
    }

    public class DeleteTodoItemRequestValidator : CustomValidator<DeleteTodoItemRequest>
    {
        public DeleteTodoItemRequestValidator(IReadRepository<Todo> repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.TodoId)
                .NotEqual(0)
                .MustAsync(async (id, ct) => await repository.AnyAsync(new TodoByIdSpec(id), ct) == true);
        }
    }

    public class DeleteTodoItemRequestHandler : IRequestHandler<DeleteTodoItemRequest, int>
    {
        private readonly DeleteTodoItemRequestValidator validator;
        private readonly IRepository<Todo> repository;

        public DeleteTodoItemRequestHandler(DeleteTodoItemRequestValidator validator, IRepository<Todo> repository)
        {
            this.validator = validator;
            this.repository = repository;
        }

        public async Task<int> Handle(DeleteTodoItemRequest request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var todo = await repository.FirstOrDefaultAsync(new TodoByIdIncludeTodoItemSpec(request.TodoId), cancellationToken);
            if (todo != null)
            {
                var todoItem = todo.TodoItems.FirstOrDefault(x => x.Id == request.TodoItemId);
                todo.RemoveTodoItem(request.TodoItemId);
                await repository.UpdateAsync(todo);

                return todo.Id;
            }
            else { throw new NotFoundException("Not Found"); }
        }
    }
}
