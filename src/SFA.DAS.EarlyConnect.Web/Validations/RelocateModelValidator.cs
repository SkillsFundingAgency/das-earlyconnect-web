using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class RelocateModelValidator : AbstractValidator<RelocateEditViewModel>
    {
        public RelocateModelValidator()
        {
            RuleFor(x => x.SelectedAnswerId).NotEqual(0).WithMessage("Select if you would move to another area of England for an apprenticeship");

        }
    }
}
