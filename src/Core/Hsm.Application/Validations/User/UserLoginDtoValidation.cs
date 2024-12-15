using FlowValidate;
using Hsm.Domain.Models.Dtos.User;
using static Hsm.Domain.Models.Constants.Constant;

namespace Hsm.Application.Validations.User
{
    public class UserLoginDtoValidation : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidation()
        {
            RuleFor(u => u.TcNumber)
                .IsNotEmpty().WithMessage(ValidationErrors.IsNullError("TcNumber"));

            RuleFor(u => u.Password)
                .IsNotEmpty().WithMessage(ValidationErrors.IsNullError("Password"));
        }
    }
}
