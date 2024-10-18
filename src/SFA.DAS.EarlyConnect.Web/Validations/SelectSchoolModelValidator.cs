using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class SelectSchoolModelValidator : AbstractValidator<SelectSchoolEditViewModel>
    {
        public SelectSchoolModelValidator()
        {
            RuleFor(x => x.SelectedSchool)
                .NotNull().WithMessage("Select a school or select 'I cannot find my school - enter manually'");
        }
    }
}