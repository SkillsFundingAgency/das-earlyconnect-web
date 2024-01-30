using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public static class Extensions
    {
        public static IRuleBuilderOptions<T, string> MatchPhoneNumber<T>(this IRuleBuilder<T, string> rule)
            => rule.Matches(@"^[\d+]+$").WithMessage("Invalid phone number")
            .MaximumLength(13).WithMessage("Phone number too long");
    }

    public class TelephoneModelValidator : AbstractValidator<TelephoneEditViewModel>
    {
        public TelephoneModelValidator()
        {
            RuleFor(x => x.Telephone).MatchPhoneNumber();
        }
    }
}
