using MediatR;

namespace Hsm.Domain.Models.Page
{
    public class BasePageQuery(int pageNumber, int pageSize)
    {
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
    }

    public class BasePagedQuery<T>(int PageNumber, int PageSize) : BasePageQuery(PageNumber, PageSize), IRequest<T>
        where T : class
    {
        public BasePagedQuery() : this(1, 10)
        {

        }
    }
}
