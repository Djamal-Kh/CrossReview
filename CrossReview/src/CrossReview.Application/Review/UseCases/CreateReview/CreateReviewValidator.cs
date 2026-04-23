using FluentValidation;

namespace CrossReview.Application.Review.UseCases.CreateReview;

public class CreateReviewValidator : AbstractValidator<CreateReviewRequest>
{
    public CreateReviewValidator()
    {
        
    }
}