using EfCore.Repository.Abstractions;
using EventFlux;
using Hsm.Application.Cqrs.Commands.Requests;
using Hsm.Application.Cqrs.Commands.Responses;
using Hsm.Domain.Entities.Entities;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Response;
using ModelMapper;

namespace Hsm.Application.Cqrs.Commands.Handlers
{
    public class CreateHospitalCommandHandler(IUnitOfWork<Hospital> _unitOfWork) : IEventHandler<CreateHospitalCommandRequest, CreateHospitalCommandResponse>
    {
        public async Task<CreateHospitalCommandResponse> Handle(CreateHospitalCommandRequest @event)
        {
            IWriteRepository<Hospital> _writeRepo = _unitOfWork.GetWriteRepository();

            Hospital hospital = ModelMap.Map<CreateHospitalDto, Hospital>(@event.CreateHospitalDto);

            await _writeRepo.AddAsync(hospital);

            bool response = await _writeRepo.SaveChangesAsync();

            return response is true ? new(ApiResponseModel<bool>.CreateSuccess(response)) : new(ApiResponseModel<bool>.CreateFailure());
        }
    }
}
