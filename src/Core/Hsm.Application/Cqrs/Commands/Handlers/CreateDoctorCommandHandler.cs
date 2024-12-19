using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Abstractions;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Doctor;
using Hsm.Domain.Models.Response;
using Microsoft.AspNetCore.Http;
using ModelMapper;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class CreateDoctorCommandHandler(IUnitOfWork<Doctor> _unitOfWork, IJwtTokenService _jwtTokenService, IHttpContextAccessor _httpContextAccessor) : IEventHandler<CreateDoctorCommandRequest, CreateDoctorCommandResponse>
    {
        public async Task<CreateDoctorCommandResponse> Handle(CreateDoctorCommandRequest @event)
        {
            IWriteRepository<Doctor> _writeRepo = _unitOfWork.GetWriteRepository();

            Doctor doctor = ModelMap.Map<CreateDoctorDto, Doctor>(@event.CreateDoctorModel);

            Guid id = Guid.Parse(_jwtTokenService.GetClaimFromRequest(_httpContextAccessor.HttpContext, "Id"));

            doctor.SetAppUserId(id);

            await _writeRepo.AddAsync(doctor);

            bool response = await _unitOfWork.SaveChangesAsync();

            return response is true ? new(ApiResponseModel<bool>.CreateSuccess(response)) : new(ApiResponseModel<bool>.CreateFailure());
        }
    }
}
