using FluentValidation;

namespace CrossReview.Application.User.UseCases.RegisterAdmin;

public class RegisterAdminValidator : AbstractValidator<RegisterAdminRequest>
{
    public RegisterAdminValidator()
    {
        
    }
}