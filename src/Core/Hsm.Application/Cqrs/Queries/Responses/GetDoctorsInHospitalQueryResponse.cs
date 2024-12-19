﻿using EventFlux;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Queries.Responses
{
    public class GetDoctorsInHospitalQueryResponse : IEventResponse
    {
        public ApiResponseModel<PageResponse<DoctorModel>> ApiResponseModel { get; set; }

        public GetDoctorsInHospitalQueryResponse(ApiResponseModel<PageResponse<DoctorModel>> apiResponseModel)
        {
            ApiResponseModel = apiResponseModel;
        }
    }
}
