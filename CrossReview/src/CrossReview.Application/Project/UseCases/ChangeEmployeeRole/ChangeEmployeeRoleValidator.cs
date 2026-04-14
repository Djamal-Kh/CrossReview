using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.ChangeEmployeeRole;

public class ChangeEmployeeRoleValidator : AbstractValidator<ChangeEmployeeRoleRequest>
{
    public ChangeEmployeeRoleValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(ChangeEmployeeRoleRequest.UserId)));
        
        RuleFor(x => x.Role)
            .IsInEnum()
            .WithError(GeneralErrors.ValueIsInvalid(nameof(ChangeEmployeeRoleRequest.Role)));
        
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(ChangeEmployeeRoleRequest.ProjectId)));
    }
}