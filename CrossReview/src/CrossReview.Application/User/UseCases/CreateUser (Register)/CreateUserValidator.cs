using FluentValidation;

namespace CrossReview.Application.User.UseCases.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        
    }
}