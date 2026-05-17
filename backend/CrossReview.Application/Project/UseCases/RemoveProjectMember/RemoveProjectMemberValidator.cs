using FluentValidation;

namespace CrossReview.Application.Project.UseCases.RemoveProjectMember;

public class RemoveProjectMemberValidator : AbstractValidator<RemoveProjectMemberRequest>
{
    public RemoveProjectMemberValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");
        
        RuleFor(x => x.ProjectId)
            .NotEqual(x => x.UserId)
            .WithMessage("Project ID and User ID cannot be the same");
    }
}