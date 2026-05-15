using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.GetReviewByParameters;

public class GetReviewByParametersUseCase
{
    private readonly ILogger<GetReviewByParametersUseCase> _logger;
    private readonly IReviewRepository _reviewRepository;
    
    public GetReviewByParametersUseCase(
        ILogger<GetReviewByParametersUseCase> logger, 
        IReviewRepository reviewRepository)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<ReviewEntity, Errors>> Execute(GetReviewByParametersRequest request,
        CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (review is null)
            return GeneralErrors.NotFound(request.Id).ToErrors();
        
        return review;
    }
}