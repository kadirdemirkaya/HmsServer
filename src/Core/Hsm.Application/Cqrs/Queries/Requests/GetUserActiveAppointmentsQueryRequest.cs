using EventFlux;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Page;

namespace Hsm.Application.Cqrs.Queries.Requests
{
    public class GetUserActiveAppointmentsQueryRequest : BasePagedQuery<GetUserActiveAppointmentsQueryResponse>
    {
        public Guid? UserId { get; set; }

        public GetUserActiveAppointmentsQueryRequest(Guid? userId)
        {
            UserId = userId;
        }
    }
}
