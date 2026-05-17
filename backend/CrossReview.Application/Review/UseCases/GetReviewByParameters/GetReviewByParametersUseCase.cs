using CrossReview.Application.Review.DTOs;
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

    public async Task<Result<ReviewDto, Errors>> Execute(GetReviewByParametersRequest request,
        CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (review is null)
            return GeneralErrors.NotFound(request.Id).ToErrors();

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