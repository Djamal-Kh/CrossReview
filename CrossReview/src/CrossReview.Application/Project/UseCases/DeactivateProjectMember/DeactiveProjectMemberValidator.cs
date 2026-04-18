using FluentValidation;

namespace CrossReview.Application.Project.UseCases.DeactivateEmployee;

public class DeactiveProjectMemberValidator : AbstractValidator<DeactivateProjectMemberRequest>
{
    public DeactiveProjectMemberValidator()
    {
        
    }
}