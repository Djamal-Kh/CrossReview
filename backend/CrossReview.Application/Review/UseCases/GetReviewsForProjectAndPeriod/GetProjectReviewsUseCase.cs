using CrossReview.Application.Review.DTOs;
using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.GetReviewsForProjectAndPeriod;

public class GetProjectReviewsUseCase
{
    private readonly IReviewRepository _reviewRepository;
    
    public GetProjectReviewsUseCase(
        IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<List<ReviewDto>, Errors>> Execute(GetProjectReviewsRequest request, CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository.GetByProject(
            request.ProjectId,
            request.RevieweeId,
            request.ReviewerId,
            request.PeriodId,
            cancellationToken);

        var result = reviews.Select(review => new ReviewDto
        {
            Id = review.Id,
            ReviewerId = review.ReviewerId,
            RevieweeId = review.RevieweeId,
            ProjectId = review.ProjectId,
            TemplateId = review.TemplateId,
            PeriodId = review.PeriodId,
            Status = review.Status,
            Answers = review.Answers.Select(a => new ReviewAnswerDto
            {
                QuestionId = a.QuestionId,
                Score = a.Score,
                Comment = a.Comment,
            }).ToList()
        }).ToList();

        return result;
    }
}