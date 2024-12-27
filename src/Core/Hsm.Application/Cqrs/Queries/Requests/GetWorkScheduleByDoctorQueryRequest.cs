using EventFlux;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Page;

namespace Hsm.Application.Cqrs.Queries.Requests
{
    public class GetWorkScheduleByDoctorQueryRequest : IEventRequest<GetWorkScheduleByDoctorQueryResponse>
    {
        public Guid DoctorId { get; set; }

        public GetWorkScheduleByDoctorQueryRequest(Guid doctorId)
        {
            DoctorId = doctorId;
        }
    }
}
