using EventFlux;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Dtos.Hospital;

namespace Hsm.Application.Cqrs.Queries.Requests
{
    public class GetHospitalQueryRequest : IEventRequest<GetHospitalQueryResponse>
    {
        public GetHospitalDto GetHospitalDto { get; set; }

        public GetHospitalQueryRequest(GetHospitalDto getHospitalDto)
        {
            GetHospitalDto = getHospitalDto;
        }
    }
}
