using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Dtos.Appointment;
using Hsm.Domain.Models.Page;

namespace Hsm.Application.Cqrs.Queries.Requests
{
    public class SearchAppointmentsQueryRequest : BasePagedQuery<SearchAppointmentsQueryResponse>
    {
        public SearchAppointmentDto SearchAppointmentDto { get; set; }

        public SearchAppointmentsQueryRequest(SearchAppointmentDto searchAppointmentDto)
        {
            SearchAppointmentDto = searchAppointmentDto;
        }
    }
}
