using FluentValidation;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.CreateReview;

public class CreateReviewValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.ReviewerId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(CreateReviewRequest.ReviewerId)));
        
        RuleFor(x => x.RevieweeId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(CreateReviewRequest.RevieweeId)));
        
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(CreateReviewRequest.ProjectId)));
        
        RuleFor(x => x.TemplateId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(CreateReviewRequest.TemplateId)));
        
        RuleFor(x => x.PeriodId)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired(nameof(CreateReviewRequest.PeriodId)));
        
        RuleFor(x => x.ReviewerId)
            .NotEqual(x => x.RevieweeId)
            .WithError(GeneralErrors.ValueIsInvalid($"{nameof(CreateReviewRequest.ReviewerId)} and {nameof(CreateReviewRequest.RevieweeId)}"));
    }
}