using FluentValidation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class DateOfBirthModelValidator : AbstractValidator<DateOfBirthEditViewModel>
    {
        public DateOfBirthModelValidator()
        {
            RuleFor(x => x.Day)
               .NotEmpty()
               .When(x => string.IsNullOrWhiteSpace(x.Month) && string.IsNullOrWhiteSpace(x.Year))
               .WithMessage("Enter your date of birth");

            RuleFor(x => x.Day)
               .NotEmpty()
               .When(x => !string.IsNullOrWhiteSpace(x.Month) || !string.IsNullOrWhiteSpace(x.Year))
               .WithMessage("Date of birth must include a day");

            RuleFor(x => x.Month)
               .NotEmpty()
               .When(x => string.IsNullOrWhiteSpace(x.Day) && string.IsNullOrWhiteSpace(x.Year))
               .WithMessage("Enter your date of birth");

            RuleFor(x => x.Month)
               .NotEmpty()
               .When(x => !string.IsNullOrWhiteSpace(x.Day) || !string.IsNullOrWhiteSpace(x.Year))
               .WithMessage("Date of birth must include a month");

            RuleFor(x => x.Year)
               .NotEmpty()
               .When(x => string.IsNullOrWhiteSpace(x.Day) && string.IsNullOrWhiteSpace(x.Month))
               .WithMessage("Enter your date of birth");

            RuleFor(x => x.Year)
               .NotEmpty()
               .When(x => !string.IsNullOrWhiteSpace(x.Day) || !string.IsNullOrWhiteSpace(x.Month))
               .WithMessage("Date of birth must include a year");


            When(x => !string.IsNullOrWhiteSpace(x.Day) && !string.IsNullOrWhiteSpace(x.Month) && !string.IsNullOrWhiteSpace(x.Year), () =>
            {
                RuleFor(x => x.Day)
                    .Must(Day => int.TryParse(Day, out int parsedDay) && parsedDay >= 1 && parsedDay <= 31)
                    .WithMessage("Date of birth must be a real date");

                RuleFor(x => x.Month)
                    .Must(Month => int.TryParse(Month, out int parsedMonth) && parsedMonth >= 1 && parsedMonth <= 12)
                    .WithMessage("Date of birth must be a real date");

                RuleFor(x => x.Year)
                    .Must(Year => int.TryParse(Year, out int parsedYear) && parsedYear.ToString().Length == 4)
                    .WithMessage("Year must include 4 number");

                RuleFor(x => x.DateOfBirth)
                    .Must(DateOfBirth => DateTime.TryParse(DateOfBirth, out DateTime parsedDate))
                    .WithMessage("Date of birth must be a real date");

                RuleFor(x => x.DateOfBirth)
                    .Must(DateOfBirth => DateTime.TryParse(DateOfBirth, out DateTime parsedDate) && parsedDate.Year < DateTime.Now.Year)
                    .WithMessage("Date of birth must be in the past");
            });
        }
    }

}