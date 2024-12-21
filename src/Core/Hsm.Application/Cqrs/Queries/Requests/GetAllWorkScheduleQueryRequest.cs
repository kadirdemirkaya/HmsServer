using EventFlux;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Page;

namespace Hsm.Application.Cqrs.Queries.Requests
{
    public class GetAllWorkScheduleQueryRequest : BasePagedQuery<GetAllWorkScheduleQueryResponse>
    {
    }
}
