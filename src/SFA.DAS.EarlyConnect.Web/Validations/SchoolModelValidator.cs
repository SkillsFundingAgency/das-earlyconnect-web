using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class SchoolModelValidator : AbstractValidator<SchoolNameEditViewModel>
    {
        public SchoolModelValidator()
        {
            RuleFor(x => x.SchoolName)
                .NotEmpty().WithMessage("Enter the name of your school or college")
                .MaximumLength(100).WithMessage("School or college name must be 100 characters or less");
        }
    }
}