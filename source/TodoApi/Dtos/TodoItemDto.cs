using TodoApi.Application.Common.Interfaces;
using TodoApi.Domain.Todos;

namespace TodoApi.Dtos
{
    public class TodoItemDto : IDto
    {

        public TodoItemDto() { }
        public TodoItemDto(TodoItem todoItem)
        {
            Id = todoItem.Id;
            Title = todoItem.Title;
            Description = todoItem.Description;
            CreatedOn = todoItem.CreatedOn;
            Done = todoItem.Done;
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool Done { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
