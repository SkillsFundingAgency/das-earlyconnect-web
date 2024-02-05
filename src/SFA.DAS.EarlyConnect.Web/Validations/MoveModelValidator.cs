using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class MoveModelValidator : AbstractValidator<MoveEditViewModel>
    {
        public MoveModelValidator()
        {
            RuleFor(x => x.Relocate).NotEmpty().WithMessage("Select if you would move to another area of England for an apprenticeship");
        }
    }
}
