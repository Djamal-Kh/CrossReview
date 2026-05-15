using FluentValidation;

namespace CrossReview.Application.Project.UseCases.UpdateProjectTitle;

public class UpdateProjectTitleValidator : AbstractValidator<UpdateProjectTitleRequest>
{
    public UpdateProjectTitleValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required");
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters")
            .MinimumLength(3)
            .WithMessage("Title must be at least 3 characters");
    }
}