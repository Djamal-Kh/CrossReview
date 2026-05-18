using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.AddAnswerToReview;

public class AddAnswerValidator : AbstractValidator<AddAnswerRequest>
{
    public AddAnswerValidator()
    {
        RuleFor(x => x.ReviewId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(AddAnswerRequest.ReviewId)))
            .NotEqual(Guid.Empty)
            .WithError(GeneralErrors.ValueIsInvalid(nameof(AddAnswerRequest.ReviewId)));
        
        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(AddAnswerRequest.QuestionId)))
            .NotEqual(Guid.Empty)
            .WithError(GeneralErrors.ValueIsInvalid(nameof(AddAnswerRequest.QuestionId)));
        
        RuleFor(x => x.Score)
            .InclusiveBetween(1, 10)
            .WithError(GeneralErrors.ValueIsInvalid(nameof(AddAnswerRequest.Score)))
            .Must(score => score >= 0)
            .WithError(GeneralErrors.ValueIsInvalid(nameof(AddAnswerRequest.Score)));
        
        RuleFor(x => x.Comment)
            .MaximumLength(1000)
            .WithError(GeneralErrors.ValueTooLong(1000, nameof(AddAnswerRequest.Comment)))
            .MaximumLength(500)
            .When(x => string.IsNullOrWhiteSpace(x.Comment))
            .WithError(GeneralErrors.ValueTooLong(500, nameof(AddAnswerRequest.Comment)));
        
        // Проверка, что комментарий не состоит только из пробелов
        RuleFor(x => x.Comment)
            .Must(comment => string.IsNullOrWhiteSpace(comment) || !string.IsNullOrWhiteSpace(comment.Trim()))
            .WithError(GeneralErrors.ValueIsInvalid(nameof(AddAnswerRequest.Comment)))
            .When(x => !string.IsNullOrWhiteSpace(x.Comment));
    }
}