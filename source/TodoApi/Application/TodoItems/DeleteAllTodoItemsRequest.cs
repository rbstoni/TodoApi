using FluentValidation;
using MediatR;
using TodoApi.Application.Common.Exceptions;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Common.Validation;
using TodoApi.Application.Todos;
using TodoApi.Domain.Todos;

namespace TodoApi.Application.TodoItems
{
    public class DeleteAllTodoItemsRequest : IRequest<int>
    {
        public int TodoId { get; set; }

        public DeleteAllTodoItemsRequest(int todoId)
        {
            TodoId = todoId;
        }
    }

    public class DeleteAllTodoItemsRequestValidator : CustomValidator<DeleteAllTodoItemsRequest>
    {
        public DeleteAllTodoItemsRequestValidator(IReadRepository<Todo> repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.TodoId)
                .NotEqual(0)
                .MustAsync(async (id, ct) => await repository.AnyAsync(new TodoByIdSpec(id), ct) == true);
        }
    }

    public class DeleteAllTodoItemsRequestHandler : IRequestHandler<DeleteAllTodoItemsRequest, int>
    {
        private readonly DeleteAllTodoItemsRequestValidator validator;
        private readonly IRepository<Todo> repository;

        public DeleteAllTodoItemsRequestHandler(DeleteAllTodoItemsRequestValidator validator, IRepository<Todo> repository)
        {
            this.validator = validator;
            this.repository = repository;
        }

        public async Task<int> Handle(DeleteAllTodoItemsRequest request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var todo = await repository.FirstOrDefaultAsync(new TodoByIdIncludeTodoItemSpec(request.TodoId), cancellationToken);
            if (todo != null)
            {
                todo.RemoveAllTodoItems();
                await repository.UpdateAsync(todo);

                return todo.Id;
            }
            else { throw new NotFoundException("Not Found"); }
        }
    }
}
