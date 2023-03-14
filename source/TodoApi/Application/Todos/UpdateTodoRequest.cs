using FluentValidation;
using MediatR;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Common.Validation;
using TodoApi.Domain.Todos;
using TodoApi.Dtos;

namespace TodoApi.Application.Todos
{
    public class UpdateTodoRequest : IRequest<TodoDto>
    {

        public UpdateTodoRequest(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public string Description { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public class UpdateTodoRequestHandler : IRequestHandler<UpdateTodoRequest, TodoDto>
    {

        private readonly IRepository<Todo> repository;
        private readonly UpdateTodoRequestValidator validator;

        public UpdateTodoRequestHandler(UpdateTodoRequestValidator validator, IRepository<Todo> repository)
        {
            this.validator = validator;
            this.repository = repository;
        }

        public async Task<TodoDto> Handle(UpdateTodoRequest request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var todo = await repository.FirstOrDefaultAsync(new TodoByIdSpec(request.Id));
            todo!.UpdateTodo(request.Name, request.Description);
            await repository.UpdateAsync(todo);

            return new TodoDto(todo);
        }

    }
    public class UpdateTodoRequestValidator : CustomValidator<UpdateTodoRequest>
    {

        public UpdateTodoRequestValidator(IReadRepository<Todo> repository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, ct) => await repository.AnyAsync(new TodoByIdSpec(id)) == true);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);
        }

    }
}
