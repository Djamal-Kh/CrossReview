using FluentValidation;

namespace CrossReview.Application.Project.UseCases.UpdateProjectDescription;

public class UpdateProjectDescriptionValidator : AbstractValidator<UpdateProjectDescriptionRequest>
{
    public UpdateProjectDescriptionValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required");
        
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description must not exceed 1000 characters");
    }
}