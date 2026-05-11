using FluentValidation;

namespace CrossReview.Application.Template.UseCases.CreateTemplate;

public class CreateTemplateValidator : AbstractValidator<CreateTemplateRequest>
{
    public CreateTemplateValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required");
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Template title is required")
            .MaximumLength(200)
            .WithMessage("Template title must not exceed 200 characters")
            .MinimumLength(3)
            .WithMessage("Template title must be at least 3 characters");
    }
}