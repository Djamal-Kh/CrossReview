using FluentValidation;

namespace CrossReview.Application.User.UseCases.Register;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        
    }
}