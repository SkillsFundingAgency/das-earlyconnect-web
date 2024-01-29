using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class NameModelValidator : AbstractValidator<NameViewModel>
    {
        public NameModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotNull()
                .WithMessage("Enter a first name")
                .MaximumLength(35)
                .WithMessage("First name must not be more than 35 characters");

            RuleFor(x => x.LastName)
                .NotNull()
                .WithMessage("Enter a last name")
                .MaximumLength(35)
                .WithMessage("Last name must not be more than 35 characters");
        }
    }
}
