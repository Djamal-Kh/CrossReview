using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.UpdateAnswerInReview;

public class UpdateAnswerUseCase
{
    private readonly ILogger<UpdateAnswerUseCase> _logger;
    private readonly IReviewRepository _reviewRepository;
    
    public UpdateAnswerUseCase(
        ILogger<UpdateAnswerUseCase> logger, 
        IReviewRepository reviewRepository)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(UpdateAnswerRequest request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);

        if (review is null)
            return GeneralErrors.NotFound(request.ReviewId).ToErrors();
        
        review.UpdateAnswer(request.QuestionId, request.Score, request.Comment);
        
        await _reviewRepository.SaveAsync(review, cancellationToken);
        
        return review.Id;
    }
}