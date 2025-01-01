using EventFlux;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Models.Dtos.WorkSchedule;

namespace Hsm.Application.Cqrs.Queries.Requests
{
    public class SearchWorkScheduleQueryRequest : IEventRequest<SearchWorkScheduleQueryResponse>
    {
        public SearchWorkScheduleDto SearcWorkScheduleDto { get; set; }

        public SearchWorkScheduleQueryRequest(SearchWorkScheduleDto searcWorkScheduleDto)
        {
            SearcWorkScheduleDto = searcWorkScheduleDto;
        }
    }
}
