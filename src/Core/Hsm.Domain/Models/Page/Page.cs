namespace Hsm.Domain.Models.Page
{
    public class Page
    {
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 10;

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRowCount { get; set; }
        public int TotalPageCount => (int)Math.Ceiling((double)TotalRowCount / PageSize);
        public int Skip => (PageNumber - 1) * PageSize;
        public bool HasNextPage => PageNumber < TotalPageCount;

        public Page() : this(0)
        {

        }

        public Page(int totalRowCount) : this(DefaultPageNumber, DefaultPageSize, totalRowCount)
        {

        }

        public Page(int pageSize, int totalRowCount) : this(DefaultPageNumber, pageSize, totalRowCount)
        {

        }

        public Page(int currentPage, int pageSize, int totalRowCount)
        {
            if (currentPage < 1)
            {
                throw new Exception("Invalid page number !");
            }

            if (pageSize < 1)
            {
                throw new Exception("Invalid ");
            }

            TotalRowCount = totalRowCount;
            PageNumber = currentPage;
            PageSize = pageSize;
        }
    }
}
