using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class EmailAddressModelValidator : AbstractValidator<EmailAddressViewModel>
    {
        public EmailAddressModelValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress()
                .WithMessage("Enter an email address in the correct format, like name @ example.com");                             
        }
    }
}
