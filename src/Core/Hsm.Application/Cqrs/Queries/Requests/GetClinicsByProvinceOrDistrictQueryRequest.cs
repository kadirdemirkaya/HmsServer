using EventFlux;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Dtos.Clinic;

namespace Hsm.Application.Cqrs.Queries.Requests
{
    public class GetClinicsByProvinceOrDistrictQueryRequest : IEventRequest<GetClinicsByProvinceOrDistrictQueryResponse>
    {
        public GetClinicDto GetClinicDto { get; set; }

        public GetClinicsByProvinceOrDistrictQueryRequest(GetClinicDto getClinicDto)
        {
            GetClinicDto = getClinicDto;
        }
    }
}
