namespace TodoApi.Application.TodoItems
{
    public class CreateTodoInput
    {

        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool Done { get; set; }

    }
}
