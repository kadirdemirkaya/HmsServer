using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Response;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class DeleteHospitalCommandHandler(IUnitOfWork<Hospital> _unitOfWork) : IEventHandler<DeleteHospitalCommandRequest, DeleteHospitalCommandResponse>
    {
        public async Task<DeleteHospitalCommandResponse> Handle(DeleteHospitalCommandRequest @event)
        {
            IWriteRepository<Hospital> _writeRepository = _unitOfWork.GetWriteRepository();
            IReadRepository<Hospital> _readRepository = _unitOfWork.GetReadRepository();

            Hospital? hospital = await _readRepository.GetAsync(h => h.Id == @event.HospitalId);

            if (hospital is null)
                return new(ApiResponseModel<bool>.CreateNotFound("Hospital id not found !"));

            hospital.SetIsActive(false);
            hospital.SetUpdatedDateUTC(DateTime.UtcNow);

            bool response = await _writeRepository.UpdateAsync(hospital);

            if (response)
            {
                return await _writeRepository.SaveChangesAsync() is true ? new(ApiResponseModel<bool>.CreateSuccess(true)) : new(ApiResponseModel<bool>.CreateFailure());
            }

            return new(ApiResponseModel<bool>.CreateServerError());
        }
    }
}
