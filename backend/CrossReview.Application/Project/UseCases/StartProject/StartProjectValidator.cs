using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.StartProject;

public class StartProjectValidator : AbstractValidator<StartProjectRequest>
{
    public StartProjectValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(StartProjectRequest.ProjectId)));
    }
}