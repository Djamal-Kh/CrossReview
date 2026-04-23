using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.AssignNewProjectMember;

public class AssignNewProjectMemberValidator : AbstractValidator<AssignNewProjectMemberRequest>
{
    public AssignNewProjectMemberValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(AssignNewProjectMemberRequest.UserId)));
        
        RuleFor(x => x.Role)
            .IsInEnum()
            .WithError(GeneralErrors.ValueIsInvalid(nameof(AssignNewProjectMemberRequest.Role)));
        
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(AssignNewProjectMemberRequest.ProjectId)));
    }
}