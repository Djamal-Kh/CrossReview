using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.RegisterAdmin;

public class RegisterAdminValidator : AbstractValidator<RegisterAdminRequest>
{
    public RegisterAdminValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(RegisterAdminRequest.FirstName)))
            .MaximumLength(100)
            .WithError(GeneralErrors.ValueTooLong(100, nameof(RegisterAdminRequest.FirstName)))
            .MinimumLength(2)
            .WithError(GeneralErrors.ValueTooShort(2, nameof(RegisterAdminRequest.FirstName)));
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(RegisterAdminRequest.LastName)))
            .MaximumLength(100)
            .WithError(GeneralErrors.ValueTooLong(100, nameof(RegisterAdminRequest.LastName)))
            .MinimumLength(2)
            .WithError(GeneralErrors.ValueTooShort(2, nameof(RegisterAdminRequest.LastName)));
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(RegisterAdminRequest.Email)))
            .EmailAddress()
            .WithError(GeneralErrors.ValueIsInvalid(nameof(RegisterAdminRequest.Email)))
            .MaximumLength(255)
            .WithError(GeneralErrors.ValueTooLong(255, nameof(RegisterAdminRequest.Email)));
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(RegisterAdminRequest.Password)))
            .MinimumLength(6)
            .WithError(GeneralErrors.ValueTooShort(6, nameof(RegisterAdminRequest.Password)))
            .MaximumLength(100)
            .WithError(GeneralErrors.ValueTooLong(100, nameof(RegisterAdminRequest.Password)))
            .Matches(@"[0-9]")
            .WithError(GeneralErrors.ValueIsInvalid(nameof(RegisterAdminRequest.Password)));
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(RegisterAdminRequest.PhoneNumber)))
            .Matches(@"^\+?[1-9][0-9]{7,14}$")
            .WithError(GeneralErrors.ValueIsInvalid(nameof(RegisterAdminRequest.PhoneNumber)));
    }
}