using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.ChangeEmployeeRole;

public class ChangeProjectMemberRoleValidator : AbstractValidator<ChangeProjectMemberRoleRequest>
{
    public ChangeProjectMemberRoleValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(ChangeProjectMemberRoleRequest.UserId)));
        
        RuleFor(x => x.Role)
            .IsInEnum()
            .WithError(GeneralErrors.ValueIsInvalid(nameof(ChangeProjectMemberRoleRequest.Role)));
        
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(ChangeProjectMemberRoleRequest.ProjectId)));
    }
}