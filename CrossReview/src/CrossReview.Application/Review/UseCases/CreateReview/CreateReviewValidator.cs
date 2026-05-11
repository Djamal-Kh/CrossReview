using FluentValidation;

namespace CrossReview.Application.Review.UseCases.CreateReview;

public class CreateReviewValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewValidator()
    {
        RuleFor(x => x.ReviewerId)
            .NotEmpty()
            .WithMessage("Reviewer ID is required");
        
        RuleFor(x => x.RevieweeId)
            .NotEmpty()
            .WithMessage("Reviewee ID is required");
        
        RuleFor(x => x.ProjectId)
            .NotEmpty()
            .WithMessage("Project ID is required");
        
        RuleFor(x => x.TemplateId)
            .NotEmpty()
            .WithMessage("Template ID is required");
        
        RuleFor(x => x.PeriodId)
            .NotEmpty()
            .WithMessage("Period ID is required");
        
        RuleFor(x => x.ReviewerId)
            .NotEqual(x => x.RevieweeId)
            .WithMessage("Reviewer and Reviewee cannot be the same person");
    }
}