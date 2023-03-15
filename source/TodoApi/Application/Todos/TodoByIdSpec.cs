using Ardalis.Specification;
using TodoApi.Domain.Todos;

namespace TodoApi.Application.Todos
{
    public class TodoByIdSpec : SingleResultSpecification<Todo>
    {
        public TodoByIdSpec(int id)
        {
            Query.Where(x => x.Id == id);
        }
    }
}
