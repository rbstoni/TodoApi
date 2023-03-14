using FluentValidation;
using MediatR;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Common.Validation;
using TodoApi.Domain.Todos;
using TodoApi.Dtos;

namespace TodoApi.Application.Todos
{
    public class CreateTodoRequest : IRequest<TodoDto>
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public CreateTodoRequest(string title, string? description)
        {
            Title = title ;
            Description = description;
        }
    }

    public class CreateTodoRequestValidator : CustomValidator<CreateTodoRequest>
    {
        public CreateTodoRequestValidator(IReadRepository<Todo> repository)
        {
            RuleFor(p => p.Title)
                .NotEmpty().WithMessage("Property name could not be empty")
                .MaximumLength(100).WithMessage("Name should be at most 100 characters")
                .MustAsync(async (name, ct) => await repository.AnyAsync(new TodoByNameSpec(name), ct) == false)
                .WithMessage((_, name) => string.Format("Name already exist", name));
        }
    }

    public class CreateTodoRequestHandler : IRequestHandler<CreateTodoRequest, TodoDto>
    {
        private readonly CreateTodoRequestValidator validator;
        private readonly IRepository<Todo> repository;

        public CreateTodoRequestHandler(CreateTodoRequestValidator validator, IRepository<Todo> repository)
        {
            this.validator = validator;
            this.repository = repository;
        }
        public async Task<TodoDto> Handle(CreateTodoRequest request, CancellationToken cancellationToken = default)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var todo = new Todo(request.Title, request.Description);
            await repository.AddAsync(todo, cancellationToken);

            return new TodoDto(todo);
        }
    }
}
