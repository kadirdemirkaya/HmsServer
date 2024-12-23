using Base.Repository.Abstractions;
using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Abstractions;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Address;
using Hsm.Domain.Models.Dtos.City;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using ModelMapper;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class UpdateHospitalCommandHandler(IReadRepository<Hospital> _readRepo,IWriteRepository<Hospital> _writeRepo) : IEventHandler<UpdateHospitalCommandRequest, UpdateHospitalCommandResponse>
    {
        public async Task<UpdateHospitalCommandResponse> Handle(UpdateHospitalCommandRequest @event)
        {
            Specification<Hospital> specification = new();
            specification.Conditions.Add(h => h.Id == @event.UpdateHospitalDto.Id);
            specification.Includes = query => query.Include(p => p.City);

            Hospital hsp = await _readRepo.GetAsync(specification);

            if (hsp is not null)
            {
                Address address = ModelMap.Map<AddressDto, Address>(@event.UpdateHospitalDto.AddressDto);
                City city = ModelMap.Map<UpdateCityDto, City>(@event.UpdateHospitalDto.UpdateCityDto);

                hsp.SetAddress(address);
                hsp.SetContactNumber(@event.UpdateHospitalDto.ContactNumber);
                hsp.SetName(@event.UpdateHospitalDto.Name);
                hsp.SetIsActive(@event.UpdateHospitalDto.IsActive);
                hsp.SetRowVersion(@event.UpdateHospitalDto.RowVersion);

                hsp.City.SetIsActive(@event.UpdateHospitalDto.UpdateCityDto.IsActive);
                hsp.City.SetUpdatedDateUTC(DateTime.UtcNow);
                hsp.City.SetRowVersion(@event.UpdateHospitalDto.UpdateCityDto.RowVersion);
            }

            bool updateHospital = await _writeRepo.UpdateAsync(hsp);

            return updateHospital is true ? new(ApiResponseModel<bool>.CreateSuccess(true)) : new(ApiResponseModel<bool>.CreateServerError());
        }
    }
}
