using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Queries.Requests;
using Hsm.Application.Cqrs.Queries.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Response;
using System.Linq.Expressions;

namespace Hsm.Application.Cqrs.Queries.Handlers
{
    public class GetHospitalQueryHandler(IReadRepository<Hospital> _readRepo) : IEventHandler<GetHospitalQueryRequest, GetHospitalQueryResponse>
    {
        public async Task<GetHospitalQueryResponse> Handle(GetHospitalQueryRequest @event)
        {
            Specification<Hospital> specification = new();
            specification.AsNoTracking = false;

            if (@event.GetHospitalDto.Province is not null)
                specification.Conditions.Add(p => p.Address.City == @event.GetHospitalDto.Province);
            if (@event.GetHospitalDto.District is not null)
                specification.Conditions.Add(p => p.Address.District == @event.GetHospitalDto.District);
            if (@event.GetHospitalDto.ClinicalId is not null)
                specification.Conditions.Add(p => p.Clinicals.Any(d => d.Id == @event.GetHospitalDto.ClinicalId));

            Expression<Func<Hospital, HospitalModel>> selectExpression = hospital => new HospitalModel
            {
                Id = hospital.Id,
                Name = hospital.Name,
                Address = Address.Create(hospital.Address.Street, hospital.Address.City, hospital.Address.State, hospital.Address.PostalCode, hospital.Address.Country, hospital.Address.District)
            };

            List<HospitalModel> hospitalModels = await _readRepo.GetListAsync(specification, selectExpression);

            return new(ApiResponseModel<List<HospitalModel>>.CreateSuccess(hospitalModels));
        }
    }
}
