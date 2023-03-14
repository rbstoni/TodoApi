using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
