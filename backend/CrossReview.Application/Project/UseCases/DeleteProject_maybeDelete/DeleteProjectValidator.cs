using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.DeleteProject_maybeDelete;

public class DeleteProjectValidator : AbstractValidator<DeleteProjectRequest>
{
    public DeleteProjectValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(DeleteProjectRequest.ProjectId)));
    }
}