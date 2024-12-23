using EventFlux;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Page;

namespace Hsm.Application.Cqrs.Queries.Requests
{
    public class GetAllHospitalsByClinicQueryRequest : BasePagedQuery<GetAllHospitalsByClinicQueryResponse>
    {
        public ClinicalHospitalsDto ClinicalHospitalsDto { get; set; }

        public GetAllHospitalsByClinicQueryRequest(ClinicalHospitalsDto clinicalHospitalsDto)
        {
            ClinicalHospitalsDto = clinicalHospitalsDto;
        }
    }
}
