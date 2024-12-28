using EventFlux;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Dtos.Doctor;

namespace Hsm.Application.Cqrs.Queries.Requests
{
    public class GetDoctorQueryRequest : IEventRequest<GetDoctorQueryResponse>
    {
        public GetDoctorDto GetDoctorDto { get; set; }

        public GetDoctorQueryRequest(GetDoctorDto getDoctorDto)
        {
            GetDoctorDto = getDoctorDto;
        }
    }
}
