using FlowValidate;
using Hsm.Domain.Models.Dtos.City;

namespace Hsm.Application.Validations.City
{
    public class CreateCityDtoValidation : AbstractValidator<CreateCityDto>
    {
        public CreateCityDtoValidation()
        {
            RuleFor(c => c.Name)
                  .IsNotEmpty().WithMessage("Name property is must not null");
        }
    }
}
