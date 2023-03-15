using FluentValidation;
using MediatR;
using TodoApi.Application.Common.Exceptions;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Common.Validation;
using TodoApi.Application.Todos;
using TodoApi.Domain.Todos;
using TodoApi.Dtos;

namespace TodoApi.Application.TodoItems
{
    public class UpdateTodoItemRequest : IRequest<TodoItemDto>
    {
        public int TodoId { get; set; }
        public int TodoItemId { get; set; }
        public CreateTodoInput Input { get; set; }

        public UpdateTodoItemRequest(int todoId, CreateTodoInput input)
        {
            TodoId = todoId;
            Input = input;
        }
    }

    public class UpdateTodoItemRequestValidator : CustomValidator<UpdateTodoItemRequest>
    {
        public UpdateTodoItemRequestValidator(IReadRepository<Todo> repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.TodoId)
                .NotEqual(0)
                .MustAsync(async (id, ct) => await repository.AnyAsync(new TodoByIdSpec(id), ct) == true);

            RuleFor(x => x.Input)
                .ChildRules(c => c.RuleFor(p => p.Title).NotEmpty());
        }
    }

    public class UpdateTodoItemRequestHandler : IRequestHandler<UpdateTodoItemRequest, TodoItemDto>
    {
        private readonly UpdateTodoItemRequestValidator validator;
        private readonly IRepository<Todo> repository;

        public UpdateTodoItemRequestHandler(UpdateTodoItemRequestValidator validator, IRepository<Todo> repository)
        {
            this.validator = validator;
            this.repository = repository;
        }

        public async Task<TodoItemDto> Handle(UpdateTodoItemRequest request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var todo = await repository.FirstOrDefaultAsync(new TodoByIdIncludeTodoItemSpec(request.TodoId), cancellationToken);
            if (todo != null)
            {
                var todoItem = todo.TodoItems.FirstOrDefault(x => x.Id == request.TodoItemId);
                todo.UpdateTodoItem(request.TodoItemId, request.Input.Title, request.Input.Description, request.Input.Done);
                await repository.UpdateAsync(todo);

                return new TodoItemDto(todoItem);
            }
            else { throw new NotFoundException("Not Found"); }
        }
    }
}
