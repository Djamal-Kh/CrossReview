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
    public async Task<List<ReviewDto>> Execute(GetReviewsForUserRequest request,
        CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository.GetAllAsync(request.UserId, request.ProjectId, request.PeriodId);
        
        var result = reviews.Select(r => new ReviewDto
        {
            Id = r.Id,
            ReviewerId = r.ReviewerId,
            RevieweeId = r.RevieweeId,
            ProjectId = r.ProjectId,
            TemplateId = r.TemplateId,
            PeriodId = r.PeriodId,
            Status = r.Status,
            Answers = r.Answers.Select(a => new ReviewAnswerDto
            {
                QuestionId = a.QuestionId,
                Score = a.Score,
                Comment = a.Comment,
            }).ToList()
        }).ToList();
        
        return result;
    }
}