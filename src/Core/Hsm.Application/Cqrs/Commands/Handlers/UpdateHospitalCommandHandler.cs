using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Response;
using ModelMapper;
using System.Net.Http.Headers;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class UpdateHospitalCommandHandler(IUnitOfWork<Hospital> _unitOfWork) : IEventHandler<UpdateHospitalCommandRequest, UpdateHospitalCommandResponse>
    {
        public async Task<UpdateHospitalCommandResponse> Handle(UpdateHospitalCommandRequest @event)
        {
            IWriteRepository<Hospital> _writeRepository = _unitOfWork.GetWriteRepository();
            IReadRepository<Hospital> _readRepository = _unitOfWork.GetReadRepository();

            bool existHospital = await _readRepository.AnyAsync(h => h.Id == @event.UpdateHospitalDto.Id);

            if (existHospital)
            {
                Hospital hospital = ModelMap.Map<UpdateHospitalDto, Hospital>(@event.UpdateHospitalDto);
                hospital.SetUpdatedDateUTC(DateTime.UtcNow);
                hospital.City.SetUpdatedDateUTC(DateTime.UtcNow);

                bool updateResponse = await _writeRepository.UpdateAsync(hospital);

                return updateResponse is true ? new(ApiResponseModel<bool>.CreateSuccess(true)) : new(ApiResponseModel<bool>.CreateFailure("Update process is not succesfully"));
            }

            return new(ApiResponseModel<bool>.CreateNotFound());
        }
    }
}
