namespace TodoApi.Application.Common.Paginations
{
    public static class PaginationFilterExtensions
    {

        public static bool HasOrderBy(this PaginationFilter filter) => filter.OrderBy?.Any() is true;

    }
    public class PaginationFilter : BaseFilter
    {

        public string[]? OrderBy { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = int.MaxValue;

    }
}