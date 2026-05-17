using FluentValidation;

namespace CrossReview.Application.User.UseCases.RegisterAdmin;

public class RegisterAdminValidator : AbstractValidator<RegisterAdminRequest>
{
    public RegisterAdminValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(100)
            .WithMessage("First name must not exceed 100 characters")
            .MinimumLength(2)
            .WithMessage("First name must be at least 2 characters");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(100)
            .WithMessage("Last name must not exceed 100 characters")
            .MinimumLength(2)
            .WithMessage("Last name must be at least 2 characters");
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email format")
            .MaximumLength(255)
            .WithMessage("Email must not exceed 255 characters");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters")
            .MaximumLength(100)
            .WithMessage("Password must not exceed 100 characters")
            .Matches(@"[0-9]")
            .WithMessage("Password must contain at least one number");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^\+?[1-9][0-9]{7,14}$")
            .WithMessage("Invalid phone number format. Expected format: +1234567890 or 1234567890");
    }
}