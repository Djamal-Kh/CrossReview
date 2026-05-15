using FluentValidation;

namespace CrossReview.Application.Template.UseCases.UpdateQuestionInTemplate;

public class UpdateQuestionValidator : AbstractValidator<UpdateQuestionRequest>
{
    public UpdateQuestionValidator()
    {
        RuleFor(x => x.TemplateId)
            .NotEmpty()
            .WithMessage("Template ID is required");
        
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .WithMessage("Question ID is required");
        
        RuleFor(x => x.Title)
            .MaximumLength(500)
            .WithMessage("Question title must not exceed 500 characters")
            .MinimumLength(3)
            .When(x => x.Title is not null)
            .WithMessage("Question title must be at least 3 characters when provided");
        
        RuleFor(x => x.Weight)
            .InclusiveBetween(0, 10)
            .When(x => x.Weight.HasValue)
            .WithMessage("Weight must be between 0 and 10 when provided");
        
        RuleFor(x => x)
            .Must(x => x.Title is not null || x.Weight is not null)
            .WithMessage("At least one field (Title or Weight) must be provided for update");
    }
}