﻿using FluentValidation;
using SFA.DAS.EarlyConnect.Web.ViewModels;

namespace SFA.DAS.EarlyConnect.Web.Validations
{
    public class PostcodeModelValidator : AbstractValidator<PostcodeEditViewModel>
    {
        public PostcodeModelValidator()
        {
            RuleFor(x => x.Postcode)
                .NotEmpty().WithMessage("Enter a full UK postcode")
                .MaximumLength(8).WithMessage("Enter a full UK postcode")
                .Matches(@"^[A-Za-z]{1,2}\d{1,2}[A-Za-z]?\s*\d[A-Za-z]{2}$")
                .WithMessage("Enter a full UK postcode");
        }
    }
}