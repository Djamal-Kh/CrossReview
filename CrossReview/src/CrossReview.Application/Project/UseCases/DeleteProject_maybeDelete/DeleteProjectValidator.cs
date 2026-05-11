using FluentValidation;

namespace CrossReview.Application.Project.UseCases.DeleteProject_maybeDelete;

public class DeleteProjectValidator : AbstractValidator<DeleteProjectRequest>
{
    public DeleteProjectValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required");
    }
}