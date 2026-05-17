using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.CloseReview;

public class CloseReviewUseCase
{
    private readonly ILogger<CloseReviewUseCase> _logger;
    private readonly IReviewRepository _reviewRepository;
    
    public CloseReviewUseCase(
        ILogger<CloseReviewUseCase> logger, 
        IReviewRepository reviewRepository)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
    }
    
    public async Task<Result<Guid, Errors>> Execute(CloseReviewRequest request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);
        
        if (review is null)
            return GeneralErrors.NotFound(request.ReviewId).ToErrors();
        
        review.Close();
        
        await _reviewRepository.SaveAsync(cancellationToken);
        
        _logger.LogInformation("Close review with id {reviewId}.", review.Id);
        
        return review.Id;
    }
}