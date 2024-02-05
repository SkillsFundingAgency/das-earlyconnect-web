using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class RelocateModelValidator : AbstractValidator<RelocateEditViewModel>
    {
        public RelocateModelValidator()
        {
            RuleFor(x => x.SelectedAnswerId).NotEqual(0).WithMessage("YourIntProperty must be zero.");
        }
    }
}
