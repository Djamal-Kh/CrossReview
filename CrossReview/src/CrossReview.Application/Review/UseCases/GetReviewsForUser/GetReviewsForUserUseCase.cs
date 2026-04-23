using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.GetReviewsForUser;

public class GetReviewsForUserUseCase
{
    private readonly ILogger<GetReviewsForUserUseCase> _logger;
    private readonly IReviewRepository _reviewRepository;
    
    public GetReviewsForUserUseCase(
        ILogger<GetReviewsForUserUseCase> logger,
        IReviewRepository reviewRepository)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
    }

    //todo надо будет возвращать DTO для сохранения конфиденциальности 
    public async Task<Result<List<ReviewEntity>, Errors>> Execute(GetReviewsForUserRequest request,
        CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository.GetAllAsync(request.UserId, request.ProjectId, request.PeriodId);

        if (!reviews.Any())
            return GeneralErrors.CollectionEmpty().ToErrors();

        //todo здесь будет еще маппинг в dto
        return reviews;
    }
}