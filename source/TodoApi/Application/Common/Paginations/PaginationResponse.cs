namespace TodoApi.Application.Common.Paginations
{
    public class PaginationResponse<T>
    {

        public PaginationResponse(List<T> data, int count, int page, int pageSize)
        {
            Data = data;
            CurrentPage = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }

        public int CurrentPage { get; set; }
        public List<T> Data { get; set; }
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasPreviousPage => CurrentPage > 1;
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

    }
}
