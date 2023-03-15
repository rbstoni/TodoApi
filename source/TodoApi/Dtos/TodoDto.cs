using TodoApi.Application.Common.Interfaces;
using TodoApi.Domain.Todos;

namespace TodoApi.Dtos
{
    public class TodoDto : IDto
    {

        public TodoDto()
        {
        }
        public TodoDto(Todo todo)
        {
            Id = todo.Id;
            Title = todo.Title;
            Description = todo.Description;
            Completed = todo.Completed;
            CreatedOn = todo.CreatedOn;
            TodoItems = todo.TodoItems.Select(x => new TodoItemDto(x)).ToArray();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedOn { get; set; }
        public IReadOnlyCollection<TodoItemDto>? TodoItems { get; set; }

    }
}
