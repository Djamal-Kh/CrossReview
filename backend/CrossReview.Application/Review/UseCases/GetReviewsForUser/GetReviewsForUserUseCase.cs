using CrossReview.Application.Review.DTOs;
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
    public async Task<List<ReviewForRevieweeDto>> Execute(GetReviewsForUserRequest request,
        CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository.GetAllAsync(request.UserId, request.ProjectId, request.PeriodId);
        
        var result = reviews.Select(r => new ReviewForRevieweeDto
        {
            Id = r.Id,
            Status = r.Status,
            Answers = r.Answers
        }).ToList();
        
        return result;
    }
}