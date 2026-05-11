using FluentValidation;

namespace CrossReview.Application.Template.UseCases.UpdateTemplateTitle;

public class UpdateTemplateTitleValidator : AbstractValidator<UpdateTemplateTitleRequest>
{
    public UpdateTemplateTitleValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty()
            .WithMessage("Template ID is required");
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Template title is required")
            .MaximumLength(200)
            .WithMessage("Template title must not exceed 200 characters")
            .MinimumLength(3)
            .WithMessage("Template title must be at least 3 characters");
    }
}