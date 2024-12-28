using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Appointment;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Dtos.WorkSchedule;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using ModelMapper;
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetClinicsByProvinceOrDistrictQueryHandler(IReadRepository<Clinical> _readRepo) : IEventHandler<GetClinicsByProvinceOrDistrictQueryRequest, GetClinicsByProvinceOrDistrictQueryResponse>
    {
        public async Task<GetClinicsByProvinceOrDistrictQueryResponse> Handle(GetClinicsByProvinceOrDistrictQueryRequest @event)
        {
            Specification<Clinical> specification = new();
            specification.AsNoTracking = false;
            specification.Includes = query => query.Include(d => d.Hospital);
            Expression<Func<Clinical, ClinicalModel>> selectExpression = clinical => new ClinicalModel
            {
                Id = clinical.Id,
                Name = clinical.Name,
                HospitalId = null
            };

            if (@event.GetClinicDto.District is not null)
                specification.Conditions.Add(a => a.Hospital.Address.District == @event.GetClinicDto.District);

            if (@event.GetClinicDto.Province is not null)
                specification.Conditions.Add(a => a.Hospital.Address.City == @event.GetClinicDto.Province);

            List<ClinicalModel> clinicalModels = await _readRepo.GetListAsync(specification, selectExpression);

            List<ClinicalModel> distinctClinicalModels = clinicalModels
                .GroupBy(c => c.Name)
                .Select(g => g.First())
                .ToList();

            return new(ApiResponseModel<List<ClinicalModel>>.CreateSuccess(distinctClinicalModels));
        }
    }
}
