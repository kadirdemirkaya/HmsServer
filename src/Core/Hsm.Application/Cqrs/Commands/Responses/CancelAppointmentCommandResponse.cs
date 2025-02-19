﻿using EventFlux;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Commands.Responses
{
    public class CancelAppointmentCommandResponse : IEventResponse
    {
        public ApiResponseModel<bool> ApiResponseModel { get; set; }

        public CancelAppointmentCommandResponse(ApiResponseModel<bool> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
