using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class AuthCodeModelValidator : AbstractValidator<AuthCodeViewModel>
    {
        public AuthCodeModelValidator()
        {
            RuleFor(x => x.AuthCode)
                .NotNull()
                .WithMessage("Enter the 6 character confirmation code.");                             
        }
    }
}
