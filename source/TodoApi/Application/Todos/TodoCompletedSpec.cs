using Ardalis.Specification;
using TodoApi.Domain.Todos;
using TodoApi.Dtos;

namespace TodoApi.Application.Todos
{
    public class TodoCompletedSpec : Specification<Todo, TodoDto>
    {
        public TodoCompletedSpec()
        {
            Query.Where(x => x.Completed == true);
        }
    }
}