using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class SearchSchoolModelValidator : AbstractValidator<SearchSchoolEditViewModel>
    {
        public SearchSchoolModelValidator()
        {
            RuleFor(x => x.SchoolSearchTerm)
                .NotEmpty().WithMessage("Enter the name of your school or college, or select 'I cannot find my school - enter manually'")
                .MaximumLength(100).WithMessage("School or college name must be 100 characters or less");

            RuleFor(x => x.SchoolSearchTerm)
                .Must((model, schoolSearchTerm) =>
                    !(model.SelectedUrn == null
                      && model.IsJsEnabled
                      && model.SchoolSearchTerm != model.ExistingSchool))
                .WithMessage("Enter the name of your school or college, or select 'I cannot find my school - enter manually'");
        }
    }
}