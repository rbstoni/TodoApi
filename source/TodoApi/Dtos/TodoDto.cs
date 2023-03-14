using TodoApi.Application.Common.Interfaces;
using TodoApi.Domain.Todos;

namespace TodoApi.Dtos
{
    public class TodoDto : IDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public decimal Progress { get; set; }
        public IReadOnlyCollection<TodoItemDto>? TodoItems { get; set; }
        public TodoDto()
        {
        }
        public TodoDto(Todo todo)
        {
            Id = todo.Id;
            Name = todo.Title;
            Description = todo.Description;
            Completed = todo.Completed;
            CreatedOn = todo.CreatedOn;
            CompletedOn = todo.CompletedOn;
            Progress = todo.Progress;
            TodoItems = todo.TodoItems.Select(x => new TodoItemDto(x)).ToArray();
        }
    }
}
