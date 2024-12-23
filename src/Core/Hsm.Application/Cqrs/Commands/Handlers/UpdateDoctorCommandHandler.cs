using Base.Repository.Abstractions;
using EfCore.Repository;
using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ModelMapper;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class UpdateDoctorCommandHandler(IBaseWriteRepository<Doctor> _writeRepo, IReadRepository<Doctor> _readRepo) : IEventHandler<UpdateDoctorCommandRequest, UpdateDoctorCommandResponse>
    {
        public async Task<UpdateDoctorCommandResponse> Handle(UpdateDoctorCommandRequest @event)
        {
            Specification<Doctor> specification = new();
            specification.Conditions.Add(h => h.Id == @event.UpdateDoctorDto.Id);
            specification.Includes = query => query.Include(p => p.Hospital).Include(w => w.AppUser).Include(w => w.WorkSchedules);

            Doctor doctor = await _readRepo.GetAsync(specification);

            doctor.SetId(@event.UpdateDoctorDto.Id);
            doctor.SetFirstName(@event.UpdateDoctorDto.FirstName);
            doctor.SetLastName(@event.UpdateDoctorDto.LastName);
            doctor.SetSpecialty(@event.UpdateDoctorDto.Specialty);
            doctor.SetSchedule(@event.UpdateDoctorDto.Schedule);
            doctor.SetIsActive(@event.UpdateDoctorDto.IsActive);
            doctor.SetRowVersion(@event.UpdateDoctorDto.RowVersion);

            bool updateResponse = await _writeRepo.UpdateAsync(doctor);

            if (!updateResponse)
                return new(ApiResponseModel<bool>.CreateFailure("Update process is not succesfully"));

            await _writeRepo.SaveChangesAsync();

            return new(ApiResponseModel<bool>.CreateSuccess(true));
        }
    }
}
