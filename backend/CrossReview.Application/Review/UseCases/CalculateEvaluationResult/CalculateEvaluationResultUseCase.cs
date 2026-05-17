using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.CalculateEvaluationResult;

public class CalculateEvaluationResultUseCase
{
    private readonly ILogger<CalculateEvaluationResultUseCase> _logger;
    private readonly IEvaluationResultRepository _evaluationResultRepository;
    private readonly IReviewRepository _reviewRepository;
    
    public CalculateEvaluationResultUseCase(
        ILogger<CalculateEvaluationResultUseCase> logger, 
        IEvaluationResultRepository evaluationResultRepository, 
        IReviewRepository reviewRepository)
    {
        _logger = logger;
        _evaluationResultRepository = evaluationResultRepository;
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(CalculateEvaluationResultRequest request, CancellationToken cancellationToken)
    {
        var reviews = await _reviewRepository.GetByReviewee(request.UserId, request.ProjectId, request.PeriodId);

        if (!reviews.Any())
            return GeneralErrors.CollectionEmpty().ToErrors();

        var result = EvaluationResultEntity.Create(request.UserId, request.ProjectId, request.PeriodId);
        
        result.Calculate(reviews);
        
        await _evaluationResultRepository.AddAsync(result, cancellationToken);
        
        _logger.LogInformation("инфа");

        return result.Id;
    }
}