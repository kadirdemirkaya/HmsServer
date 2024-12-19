using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class DeleteDoctorCommandHandler(IWriteRepository<Doctor> _writeRepo, IReadRepository<Doctor> _readRepo) : IEventHandler<DeleteDoctorCommandRequest, DeleteDoctorCommandResponse>
    {
        public async Task<DeleteDoctorCommandResponse> Handle(DeleteDoctorCommandRequest @event)
        {
            Doctor? existDoctor = await _readRepo.GetAsync(d => d.Id == @event.Id);

            if (existDoctor is not null)
            {
                bool response = await _writeRepo.DeleteAsync(existDoctor);
                return response is true ? new(ApiResponseModel<bool>.CreateSuccess(response)) : new(ApiResponseModel<bool>.CreateFailure());
            }
            return new(ApiResponseModel<bool>.CreateNotFound());
        }
    }
}
