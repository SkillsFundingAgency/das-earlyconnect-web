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
                .MaximumLength(150)
                .WithMessage("Enter a first name");

            RuleFor(x => x.FirstName)
                .NotNull()
                .MaximumLength(150)
                .WithMessage("Enter a last name");
        }
    }
}
