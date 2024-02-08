using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class EmailAddressModelValidator : AbstractValidator<EmailAddressViewModel>
    {
        public EmailAddressModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Enter an email address in the correct format, like name@example.com")
                .Matches(@"^[^@\s]+@[^@\s]+\.[a-zA-Z]{2,4}$")
                .WithMessage("Enter an email address in the correct format, like name@example.com");
        }
    }
}
