using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.CloseProject;

public class CloseProjectValidator : AbstractValidator<CloseProjectRequest>
{
    public CloseProjectValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(CloseProjectRequest.ProjectId)));
    }
}