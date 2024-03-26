using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;
using System.Text.RegularExpressions;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class PostcodeModelValidator : AbstractValidator<PostcodeEditViewModel>
    {
        public PostcodeModelValidator()
        {
            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Enter a full UK postcode")
                .MaximumLength(15).WithMessage("Enter a full UK postcode")
                .Must(BeAValidUKPostcode)
                .WithMessage("Enter a valid UK postcode");
        }

        private bool BeAValidUKPostcode(string postcode)
        {
            string cleanedPostcode = Regex.Replace(postcode, @"[-()\[\]{}<>\.\s]", "");

            if (cleanedPostcode.Length > 8)
            {
                return false;
            }

            return Regex.IsMatch(cleanedPostcode, @"^[A-Za-z]{1,2}\d{1,2}[A-Za-z]?\s*\d[A-Za-z]{2}$");
        }
    }
}