using CrossReview.Application.Review.DTOs;
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

    public async Task<Result<ReviewDto, Errors>> Execute(GetProjectReviewsRequest request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByProject(request.ProjectId, request.RevieweeId, request.ReviewerId,
            request.PeriodId, cancellationToken);

        if (review is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        var answerDtos = review.Answers
            .Select(r => new ReviewAnswerDto
            {
                QuestionId = r.QuestionId,
                Score = r.Score,
                Comment = r.Comment,
            }).ToList();
        
        var result = new ReviewDto
        {
            Id =  review.Id,
            ReviewerId = review.ReviewerId,
            RevieweeId = review.RevieweeId,
            ProjectId = review.ProjectId,
            TemplateId = review.TemplateId,
            PeriodId = review.PeriodId,
            Status = review.Status,
            Answers = answerDtos
        };
        
        return result;
    }
}