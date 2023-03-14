using Ardalis.Specification;
using TodoApi.Domain.Todos;

namespace TodoApi.Application.Todos
{
    public class TodoByNameSpec : Specification<Todo>, ISingleResultSpecification
    {
        public TodoByNameSpec(string name)
        {
            Query.Where(t => t.Title == name);
        }
    }
}
