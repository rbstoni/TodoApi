﻿using FluentValidation;
using MediatR;
using TodoApi.Application.Common.Exceptions;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Common.Validation;
using TodoApi.Application.Todos;
using TodoApi.Domain.Todos;
using TodoApi.Dtos;

namespace TodoApi.Application.TodoItems
{
    public class CreateTodoItemRequest : IRequest<TodoItemDto>
    {
        public int TodoId { get; set; }
        public CreateTodoInput Input { get; set; }

        public CreateTodoItemRequest(int todoId, CreateTodoInput input)
        {
            TodoId = todoId;
            Input = input;
        }
    }

    public class CreateTodoItemRequestValidator : CustomValidator<CreateTodoItemRequest>
    {
        public CreateTodoItemRequestValidator(IReadRepository<Todo> repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.TodoId)
                .NotEqual(0)
                .MustAsync(async (id, ct) => await repository.AnyAsync(new TodoByIdSpec(id), ct) == true);

            RuleFor(x => x.Input)
                .ChildRules(c => c.RuleFor(p => p.Title).NotEmpty());
        }
    }

    public class CreateTodoItemRequestHandler : IRequestHandler<CreateTodoItemRequest, TodoItemDto>
    {
        private readonly CreateTodoItemRequestValidator validator;
        private readonly IRepository<Todo> repository;

        public CreateTodoItemRequestHandler(CreateTodoItemRequestValidator validator, IRepository<Todo> repository)
        {
            this.validator = validator;
            this.repository = repository;
        }

        public async Task<TodoItemDto> Handle(CreateTodoItemRequest request, CancellationToken cancellationToken)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var todo = await repository.FirstOrDefaultAsync(new TodoByIdIncludeTodoItemSpec(request.TodoId), cancellationToken);
            if (todo != null)
            {
                todo.AddTodoItem(request.Input.Title!, request.Input.Description, request.Input.Done);
                await repository.UpdateAsync(todo);
                var todoItems = todo.TodoItems.Last();

                return new TodoItemDto(todoItems);
            }
            else { throw new NotFoundException("Not Found"); }
        }
    }
}
