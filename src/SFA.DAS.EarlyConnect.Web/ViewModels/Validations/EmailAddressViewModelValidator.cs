using FluentValidation;
using System.Data;

namespace SFA.DAS.EarlyConnect.Web.ViewModels.Validations
{
    public class EmailAddressViewModelValidator:AbstractValidator<EmailAddressViewModel>
    {
        public EmailAddressViewModelValidator() 
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email canno be empty")
                .EmailAddress()
                .WithMessage("A valid email is required");
        }
    }
}
