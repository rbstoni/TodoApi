using FluentValidation;
using MediatR;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Common.Validation;
using TodoApi.Domain.Todos;

namespace TodoApi.Application.Todos
{
    public class DeleteTodoRequest : IRequest<int>
    {
        public int Id { get; set; }
        public DeleteTodoRequest(int id)
        {
            Id = id;
        }
    }

    public class DeleteTodoRequestValidator : CustomValidator<DeleteTodoRequest>
    {
        public DeleteTodoRequestValidator(IReadRepository<Todo> repository)
        {
            RuleFor(p => p.Id)
                .NotEqual(0)
                .MustAsync((async (id, ct) => await repository.AnyAsync(new TodoByIdIncludeTodoItemSpec(id)) == true));
        }
    }

    public class DeleteTodoRequestHandler : IRequestHandler<DeleteTodoRequest, int>
    {
        private readonly IRepository<Todo> repository;
        private readonly DeleteTodoRequestValidator validator;

        public DeleteTodoRequestHandler(IRepository<Todo> repository, DeleteTodoRequestValidator validator)
        {
            this.repository = repository;
            this.validator = validator;
        }
        public async Task<int> Handle(DeleteTodoRequest request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var todo = await repository.FirstOrDefaultAsync(new TodoByIdIncludeTodoItemSpec(request.Id));
            await repository.DeleteAsync(todo);

            return request.Id;
        }
    }
}
