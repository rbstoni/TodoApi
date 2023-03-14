using FluentValidation;
using MediatR;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Common.Validation;
using TodoApi.Domain.Todos;
using TodoApi.Dtos;

namespace TodoApi.Application.Todos
{
    public class GetTodoRequest : IRequest<TodoDto>
    {
        public int Id { get; set; }
        public GetTodoRequest(int id)
        {
            Id = id;
        }
    }

    public class GetTodoRequestValidator : CustomValidator<GetTodoRequest>
    {
        public GetTodoRequestValidator(IReadRepository<Todo> repository)
        {
            RuleFor(x => x.Id)
                .MustAsync(async (id, ct) => await repository.AnyAsync(new TodoByIdSpec(id)) == true);
        }
    }

    public class GetTodoRequestHandler : IRequestHandler<GetTodoRequest, TodoDto>
    {
        private readonly GetTodoRequestValidator validator;
        private readonly IReadRepository<Todo> repository;

        public GetTodoRequestHandler(GetTodoRequestValidator validator, IReadRepository<Todo> repository)
        {
            this.validator = validator;
            this.repository = repository;
        }

        public async Task<TodoDto> Handle(GetTodoRequest request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var todo = await repository.FirstOrDefaultAsync(new TodoByIdSpec(request.Id));

            return new TodoDto(todo!);
        }
    }
}
