using FluentValidation;

namespace CrossReview.Application.Review.UseCases.AddAnswerToReview;

public class AddAnswerValidator : AbstractValidator<AddAnswerRequest>
{
    public AddAnswerValidator()
    {
        RuleFor(x => x.ReviewId)
            .NotEmpty()
            .WithMessage("Review ID is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Review ID must be a valid GUID");
        
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .WithMessage("Question ID is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Question ID must be a valid GUID");
        
        RuleFor(x => x.Score)
            .InclusiveBetween(1, 10)
            .WithMessage("Score must be between 1 and 10")
            .Must(score => score >= 0)
            .WithMessage("Score cannot be negative");
        
        RuleFor(x => x.Comment)
            .MaximumLength(1000)
            .WithMessage("Comment must not exceed 1000 characters")
            .MaximumLength(500)
            .When(x => string.IsNullOrWhiteSpace(x.Comment))
            .WithMessage("Comment is required when score is below 5"); // Пример условной валидации
        
        // Проверка, что комментарий не состоит только из пробелов
        RuleFor(x => x.Comment)
            .Must(comment => string.IsNullOrWhiteSpace(comment) || !string.IsNullOrWhiteSpace(comment.Trim()))
            .WithMessage("Comment cannot consist only of whitespace characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Comment));
    }
}