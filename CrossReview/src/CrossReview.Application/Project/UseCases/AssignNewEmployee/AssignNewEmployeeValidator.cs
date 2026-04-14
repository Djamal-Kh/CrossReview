using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.AssignEmployee;

public class AssignNewEmployeeValidator : AbstractValidator<AssignNewEmployeeRequest>
{
    public AssignNewEmployeeValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(AssignNewEmployeeRequest.UserId)));
        
        RuleFor(x => x.Role)
            .IsInEnum()
            .WithError(GeneralErrors.ValueIsInvalid(nameof(AssignNewEmployeeRequest.Role)));
        
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(AssignNewEmployeeRequest.ProjectId)));
    }
}