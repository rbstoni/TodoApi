using TodoApi.Application.Todos;
using TodoApi.Domain.Common;

namespace TodoApi.Domain.Todos
{
    public class Todo : BaseEntity, IAggregateRoot
    {

        private decimal _progress;
        private List<TodoItem> todoItems = new();
        public Todo()
        {

        }
        public Todo(string title, string? description)
        {
            Title = title;
            Description = description;
            Progress = GetProgress();
        }

        #region Properties
        public bool Completed
        {
            get => Progress == 100;
            set
            {
                if (value == true && Completed == false)
                {
                    CompletedOn = DateTime.UtcNow;
                }
            }
        }
        public DateTime? CompletedOn { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Progress
        {
            get => _progress;
            private set => _progress = value;
        }
        public IReadOnlyCollection<TodoItem> TodoItems => todoItems.AsReadOnly();
        #endregion

        #region Todo
        public void UpdateTodo(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name) == false && Title?.Equals(name) == false)
            {
                Title = name;
            }
            Description = description;
        }
        #endregion
        private decimal GetProgress()
        {
            var count = todoItems.Count;
            var completed = todoItems.Count(x => x.Done == true);

            return count == 0 ? 0 : count / completed;
        }

        #region Todo Items
        public void AddTodoItem(string title, string? description, bool done = false)
        {
            var todoItem = new TodoItem(title, description, done);
            todoItems.Add(todoItem);
        }
        public void UpdateTodoItem(int todoItemId, string title, string? description, bool done = false)
        {
            var todoItem = todoItems.Find(x => x.Id == todoItemId);
            if (todoItem != null)
            {
                todoItem.Title = title;
                todoItem.Description = description;
                todoItem.Done = done;
            }
        }
        public void RemoveTodoItem(int todoItemId)
        {
            var todoItem = todoItems.Find(x => x.Id == todoItemId);
            if (todoItem != null)
            {
                todoItems.Remove(todoItem);
            }
        }
        public void RemoveAllTodoItems()
        {
            todoItems.Clear();
        }
        #endregion


    }
}
