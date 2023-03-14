using Ardalis.Specification;
using TodoApi.Domain.Todos;

namespace TodoApi.Application.Todos
{
    public class TodoByIdSpec : Specification<Todo>, ISingleResultSpecification
    {
        public TodoByIdSpec(int id)
        {
            Query.Where(x => x.Id == id).Include(x=>x.TodoItems);
        }
    }
}
