using FlowValidate;
using Hsm.Application.Validations.Address;
using Hsm.Application.Validations.City;
using Hsm.Domain.Models.Dtos.Hospital;
using Hsm.Domain.Models.Dtos.User;

namespace Hsm.Application.Validations.Hospital
{
    public class CreateHospitalDtoValidation : AbstractValidator<CreateHospitalDto>
    {
        public CreateHospitalDtoValidation()
        {
            RuleFor(h => h.ContactNumber)
                .IsNotEmpty().WithMessage("ContactNumber property is must not null");

            RuleFor(h => h.Name)
                 .IsNotEmpty().WithMessage("Name property is must not null");

            ValidateNested(h => h.Address, new AddressValidation());

            ValidateNested(h => h.CreateCityDto, new CreateCityDtoValidation());
        }
    }
}
