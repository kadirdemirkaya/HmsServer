using EventFlux;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Page;

namespace Hsm.Application.Cqrs.Queries.Requests
{
    public class GetDoctorsInHospitalQueryRequest : BasePagedQuery<GetDoctorsInHospitalQueryResponse>
    {
        public Guid HospitalId { get; set; }

        public GetDoctorsInHospitalQueryRequest(Guid hospitalId)
        {
            HospitalId = hospitalId;
        }
    }
}
