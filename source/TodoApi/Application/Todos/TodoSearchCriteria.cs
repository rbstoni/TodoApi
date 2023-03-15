using Swashbuckle.AspNetCore.Annotations;

namespace TodoApi.Application.Todos
{
    /// <summary>
    /// Filters to search to do
    /// </summary>
    public class TodoSearchCriteria
    {
        [SwaggerSchema(Description = "Todo Id")]
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? Completed { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? CompletedOn { get; set; }
        public decimal? Progress { get; set; }
    }
}
