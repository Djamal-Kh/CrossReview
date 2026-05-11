using FluentValidation;

namespace CrossReview.Application.Template.UseCases.AddQuestionToTemplate;

public class AddQuestionValidator : AbstractValidator<AddQuestionRequest>
{
    public AddQuestionValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty()
            .WithMessage("Template ID is required");
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Question title is required")
            .MaximumLength(500)
            .WithMessage("Question title must not exceed 500 characters")
            .MinimumLength(3)
            .WithMessage("Question title must be at least 3 characters");
        
        RuleFor(x => x.Weight)
            .InclusiveBetween(0, 10)
            .WithMessage("Weight must be between 0 and 10")
            .Must(weight => weight >= 0)
            .WithMessage("Weight cannot be negative");
    }
}