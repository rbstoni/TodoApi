namespace TodoApi.Dtos
{
    public class TodoItemSearchCriteria
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public bool? Done { get; set; }
    }
}
