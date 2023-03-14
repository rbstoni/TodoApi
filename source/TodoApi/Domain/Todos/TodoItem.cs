using TodoApi.Domain.Common;
using TodoApi.Dtos;

namespace TodoApi.Domain.Todos
{
    public class TodoItem : BaseEntity
    {
        public int TodoId { get; set; }
        public Todo? Todo { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool Done { get; set; }

        public TodoItem(string title, string? description, bool done = false)
        {
            Title = title;
            Description = description;
            Done = done;
        }

    }
}
