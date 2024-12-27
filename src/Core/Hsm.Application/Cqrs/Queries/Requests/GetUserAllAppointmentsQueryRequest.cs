using EventFlux;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Page;

namespace Hsm.Application.Cqrs.Queries.Requests
{
    public class GetUserAllAppointmentsQueryRequest : BasePagedQuery<GetUserAllAppointmentsQueryResponse>
    {
        public Guid? UserId { get; set; }
        public GetUserAllAppointmentsQueryRequest(Guid? userId)
        {
            UserId = userId;
        }
    }
}
