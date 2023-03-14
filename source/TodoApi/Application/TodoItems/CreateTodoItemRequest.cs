using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.Application.Common.Persistence;
using TodoApi.Application.Todos;
using TodoApi.Domain.Todos;
using TodoApi.Dtos;

namespace TodoApi.Application.TodoItems
{
    public class CreateTodoItemRequest : IRequest<int>
    {
        public int TodoId { get; set; }
        public CreateTodoInput Input { get; set; }

        public CreateTodoItemRequest(int todoId, CreateTodoInput input)
        {
            TodoId = todoId;
            Input = input;
        }
    }

    public class CreateTodoItemRequestHandler : IRequestHandler<CreateTodoItemRequest, int>
    {
        private readonly IRepository<Todo> repository;

        public CreateTodoItemRequestHandler(IRepository<Todo> repository)
        {
            this.repository = repository;
        }

        public async Task<int> Handle(CreateTodoItemRequest request, CancellationToken cancellationToken)
        {
            var todo = await repository.FirstOrDefaultAsync(new TodoByIdSpec(request.TodoId));
            todo?.AddTodoItem(request.Input.Title, request.Input.Description, request.Input.Done);
            await repository.UpdateAsync(todo!);
            
            return request.TodoId;
        }
    }
}
