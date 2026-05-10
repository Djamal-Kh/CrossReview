using FluentValidation;

namespace CrossReview.Application.User.UseCases.CreateUser__Register_;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        
    }
}