using Ardalis.Specification;
using TodoApi.Domain.Todos;

namespace TodoApi.Application.Todos
{
    public class TodoByIdIncludeTodoItemSpec : Specification<Todo>, ISingleResultSpecification
    {
        public TodoByIdIncludeTodoItemSpec(int id)
        {
            Query.Where(x => x.Id == id).Include(x => x.TodoItems);
        }
    }
}
