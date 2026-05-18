using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.RemoveProjectMember;

public class RemoveProjectMemberValidator : AbstractValidator<RemoveProjectMemberRequest>
{
    public RemoveProjectMemberValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(RemoveProjectMemberRequest.ProjectId)));
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(RemoveProjectMemberRequest.UserId)));
    }
}