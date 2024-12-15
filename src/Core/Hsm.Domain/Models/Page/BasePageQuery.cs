using EventFlux;

namespace Hsm.Domain.Models.Page
{
    public class BasePageQuery(int pageNumber, int pageSize)
    {
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
    }

    public class BasePagedQuery<T> : BasePageQuery, IEventRequest<T>
        where T : IEventResponse
    {
        public BasePagedQuery(int PageNumber, int PageSize) : base(PageNumber, PageSize)
        {
        }

        public BasePagedQuery() : this(1, 10)
        {

        }
    }
}
