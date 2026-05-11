using FluentValidation;

namespace CrossReview.Application.Project.UseCases.StartProject;

public class StartProjectValidator : AbstractValidator<StartProjectRequest>
{
    public StartProjectValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required");
    }
}