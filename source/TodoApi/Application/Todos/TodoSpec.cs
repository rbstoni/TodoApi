using Ardalis.Specification;
using TodoApi.Domain.Todos;
using TodoApi.Dtos;

namespace TodoApi.Application.Todos
{
    public class TodoSpec : Specification<Todo, TodoDto>
    {

        public TodoSpec()
        {
            Query.Include(x => x.TodoItems);
        }

    }
}
