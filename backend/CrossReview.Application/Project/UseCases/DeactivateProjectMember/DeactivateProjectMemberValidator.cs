using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.DeactivateProjectMember;

public class DeactivateProjectMemberValidator : AbstractValidator<DeactivateProjectMemberRequest>
{
    public DeactivateProjectMemberValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(DeactivateProjectMemberRequest.ProjectId)));
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(DeactivateProjectMemberRequest.UserId)));
    }
}