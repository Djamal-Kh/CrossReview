using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.RegisterUser;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(RegisterUserRequest.FirstName)))
            .MaximumLength(100)
            .WithError(GeneralErrors.ValueIsInvalid("First name must not exceed 100 characters"))
            .MinimumLength(2)
            .WithError(GeneralErrors.ValueIsInvalid("First name must be at least 2 characters"));
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(RegisterUserRequest.LastName)))
            .MaximumLength(100)
            .WithError(GeneralErrors.ValueIsInvalid("Last name must not exceed 100 characters"))
            .MinimumLength(2)
            .WithError(GeneralErrors.ValueIsInvalid("Last name must be at least 2 characters"));
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(RegisterUserRequest.Email)))
            .EmailAddress()
            .WithError(GeneralErrors.ValueIsInvalid("Invalid email format"))
            .MaximumLength(255)
            .WithError(GeneralErrors.ValueIsInvalid("Email must not exceed 255 characters"));
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(RegisterUserRequest.Password)))
            .MinimumLength(6)
            .WithError(GeneralErrors.ValueIsInvalid("Password must be at least 6 characters"))
            .MaximumLength(100)
            .WithError(GeneralErrors.ValueIsInvalid("Password must not exceed 100 characters"))
            .Matches(@"[0-9]")
            .WithError(GeneralErrors.ValueIsInvalid("Password must contain at least one number"));
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(RegisterUserRequest.PhoneNumber)))
            .Matches(@"^\+?[1-9][0-9]{7,14}$")
            .WithError(GeneralErrors.ValueIsInvalid("Invalid phone number format. Expected format: +1234567890 or 1234567890"));
    }
}