using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Response;
using ModelMapper;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class UpdateDoctorCommandHandler(IWriteRepository<Doctor> _writeRepo, IReadRepository<Doctor> _readRepo) : IEventHandler<UpdateDoctorCommandRequest, UpdateDoctorCommandResponse>
    {
        public async Task<UpdateDoctorCommandResponse> Handle(UpdateDoctorCommandRequest @event)
        {
            bool existDoctor = await _readRepo.AnyAsync(d => d.Id == @event.UpdateDoctorDto.Id);

            if(existDoctor)
            {
                Doctor doctor = ModelMap.Map<UpdateDoctorDto, Doctor>(@event.UpdateDoctorDto);
                doctor.SetUpdatedDateUTC(DateTime.UtcNow);

                bool updateResponse = await _writeRepo.UpdateAsync(doctor);

                return updateResponse is true ? new(ApiResponseModel<bool>.CreateSuccess(true)) : new(ApiResponseModel<bool>.CreateFailure("Update process is not succesfully"));
            }

            return new(ApiResponseModel<bool>.CreateNotFound());
        }
    }
}
