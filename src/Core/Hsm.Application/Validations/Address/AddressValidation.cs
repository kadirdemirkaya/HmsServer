using FlowValidate;

namespace Hsm.Application.Validations.Address
{
    public class AddressValidation : AbstractValidator<Hsm.Domain.Entities.Entities.Address>
    {
        public AddressValidation()
        {
            RuleFor(a => a.City)
                .IsNotEmpty().WithMessage("City property is must not null");

            RuleFor(a => a.Country)
                .IsNotEmpty().WithMessage("Country property is must not null");

            RuleFor(a => a.PostalCode)
                .IsNotEmpty().WithMessage("PostalCode property is must not null");

            RuleFor(a => a.State)
              .IsNotEmpty().WithMessage("State property is must not null");

            RuleFor(a => a.Street)
              .IsNotEmpty().WithMessage("Street property is must not null");
        }
    }
}
