using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.RemoveAnswerFromReview;

public class RemoveAnswerUseCase
{
    private readonly ILogger<RemoveAnswerUseCase> _logger;
    private readonly IReviewRepository _reviewRepository;
    
    public RemoveAnswerUseCase(
        ILogger<RemoveAnswerUseCase> logger,
        IReviewRepository reviewRepository)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(RemoveAnswerRequest request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);

        if (review is null)
            return GeneralErrors.NotFound(request.ReviewId).ToErrors();
        
        review.RemoveAnswer(request.QuestionId);

        await _reviewRepository.SaveAsync(review, cancellationToken);
        
        _logger.LogInformation("инфа");

        return review.Id;
    }
}