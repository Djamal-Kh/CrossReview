using FluentValidation;

namespace CrossReview.Application.User.UseCases.CreateUser__Register_;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        
    }
}