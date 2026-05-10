using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.GetReviewsForProjectAndPeriod;

public class GetProjectReviewsUseCase
{
    private readonly ILogger<GetProjectReviewsUseCase> _logger;
    private readonly IReviewRepository _reviewRepository;
    
    public GetProjectReviewsUseCase(
        ILogger<GetProjectReviewsUseCase> logger, 
        IReviewRepository reviewRepository)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<ReviewEntity, Errors>> Execute(GetProjectReviewsRequest request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByProject(request.ProjectId, request.RevieweeId, request.ReviewerId,
            request.PeriodId, cancellationToken);

        if (review is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        return review;
    }
}