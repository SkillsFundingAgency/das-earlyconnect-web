using FluentValidation;
using PhoneNumbers;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public static class Extensions
    {     
        public static IRuleBuilderOptions<T, string> MatchPhoneNumber<T>(this IRuleBuilder<T, string> rule)
        => rule
        .Must(BeValidPhoneNumber).WithMessage("Enter a telephone number, like 01632 960 001, 07700 900 982 or + 44 808 157 0192");

        private static bool BeValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return true;
            }

            try
            {
                var phoneUtil = PhoneNumberUtil.GetInstance();
                var parsedNumber = phoneUtil.Parse(phoneNumber.Trim(), "GB");
                return phoneUtil.IsValidNumber(parsedNumber);
            }
            catch 
            {
                return false;
            }
        }
    }

    public class TelephoneModelValidator : AbstractValidator<TelephoneEditViewModel>
    {
        public TelephoneModelValidator()
        {
            RuleFor(x => x.Telephone).MatchPhoneNumber();
        }

        
    }
}
