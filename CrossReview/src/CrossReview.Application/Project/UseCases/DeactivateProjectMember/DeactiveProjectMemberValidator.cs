using FluentValidation;

namespace CrossReview.Application.Project.UseCases.DeactivateProjectMember;

public class DeactiveProjectMemberValidator : AbstractValidator<DeactivateProjectMemberRequest>
{
    public DeactiveProjectMemberValidator()
    {
        
    }
}