using Base.Repository.Abstractions;
using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Application.Extensions;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Clinic;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Page;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using ModelMapper;
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetAllHospitalQueryHandler(IReadRepository<Hospital> _readRepo) : IEventHandler<GetAllHospitalQueryRequest, GetAllHospitalQueryResponse>
    {
        public async Task<GetAllHospitalQueryResponse> Handle(GetAllHospitalQueryRequest @event)
        {
            Specification<Hospital> specification = new();
            specification.AsNoTracking = false;
            specification.Skip = @event.PageSize;
            specification.Take = @event.PageNumber;
            specification.Includes = query => query.Include(c => c.City).Include(h => h.Address).Include(d => d.Clinicals);

            Expression<Func<Hospital, HospitalModel>> selectExpression = hospital => new HospitalModel
            {
                Id = hospital.Id,
                Address = Address.Create(hospital.Address.Street, hospital.Address.City, hospital.Address.State, hospital.Address.PostalCode, hospital.Address.Country, hospital.Address.District),
                CityModel = new()
                {
                    Id = hospital.City.Id,
                    IsActive = hospital.City.IsActive,
                    Name = hospital.City.Name,
                    RowVersion = hospital.City.RowVersion
                },
                ContactNumber = hospital.ContactNumber,
                IsActive = hospital.IsActive,
                Name = hospital.Name,
                RowVersion = hospital.RowVersion,
                ClinicalModels = hospital.Clinicals.Select(clinical => new ClinicalModel
                {
                    Id = clinical.Id,
                    Name = clinical.Name,
                    HospitalId = clinical.HospitalId
                }).ToList()
            };

            List<HospitalModel> doctorModels = await _readRepo.GetListAsync(specification, selectExpression);

            if (doctorModels is null)
                return new(ApiResponseModel<PageResponse<HospitalModel>>.CreateNotFound("Doctors not found"));

            PageResponse<HospitalModel> pageResponse = new(doctorModels, new()
            {
                PageNumber = @event.PageNumber,
                PageSize = @event.PageSize,
                TotalRowCount = doctorModels.Count()
            });

            return new(ApiResponseModel<PageResponse<HospitalModel>>.CreateSuccess(pageResponse));
        }
    }
}
