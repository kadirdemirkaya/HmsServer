using EventFlux;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Page;

namespace Hsm.Application.Cqrs.Queries.Requests
{
    public class GetAllDoctorsByClinicalRequest : BasePagedQuery<GetAllDoctorsByClinicalResponse>
    {
        public ClinicalDoctorDto ClinicalDoctorDto { get; set; }

        public GetAllDoctorsByClinicalRequest(ClinicalDoctorDto clinicalDoctorDto)
        {
            ClinicalDoctorDto = clinicalDoctorDto;
        }
    }
}
