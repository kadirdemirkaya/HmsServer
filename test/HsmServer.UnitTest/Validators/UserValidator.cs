using FlowValidate;
using HsmServer.UnitTest.Models;

namespace HsmServer.UnitTest.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .Must(name => name.Length >= 3).WithMessage("Name must be big than 2 character");

            RuleFor(user => user.Emails)
                .Should(emails =>
                {
                    foreach (var email in emails)
                    {
                        if (!email.Contains("@"))
                            throw new Exception("Email format is wrong");
                        if (email is null)
                            throw new Exception("Email is null");
                    }
                }, "Email format is wrong");

            RuleFor(user => user)
                .Must(user =>
                {
                    if (!user.Name.Contains("k"))
                        return false;
                    return true;
                }).WithMessage("User name must be contains 'k' character")
                .Must(user =>
                {
                    if (user.Name.Length <= 10)
                        return false;
                    return true;
                }).WithMessage("User name length at least be than 10");

            ValidateNested(user => user.UserCustomer, new UserProductsValidator());
        }
    }

    public class UserProductsValidator : AbstractValidator<UserCustomer>
    {
        public UserProductsValidator()
        {
            RuleFor(product => product.Email)
                .IsNotEmpty().WithMessage("ProductCustomer Email is null")
                .IsEmail().WithMessage("ProductCustomer not Email format");

            RuleFor(product => product.PhoneNumber)
                .IsNotEmpty().WithMessage("ProductCustomer PhoneNumber is null")
                .Length(10, 11).WithMessage("ProductCustomer not PhoneNumber format");
        }
    }

    public class UserBasketsValidator : AbstractValidator<UserBasket>
    {
        public UserBasketsValidator()
        {
            RuleFor(ubas => ubas.Count)
                .IsNotEmpty().WithMessage("[user basket] count is null !")
                .IsGreaterThan(0).WithMessage("[user basket] count have to be grater then 5");

            RuleFor(ubas => ubas.Name)
                 .IsNotEmpty().WithMessage("[user basket] name is null")
                 .Length(1, 10).WithMessage("[user basket] name havbe to be between 1 and 10");
        }
    }
}
