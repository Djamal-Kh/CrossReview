using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.RecalculateEvaluationResult;

public class RecalculateEvaluationResultUseCase
{
    private readonly ILogger<RecalculateEvaluationResultUseCase> _logger;
    private readonly IEvaluationResultRepository _evaluationResultRepository;
    private readonly IReviewRepository _reviewRepository;
    
    public RecalculateEvaluationResultUseCase(
        ILogger<RecalculateEvaluationResultUseCase> logger, 
        IEvaluationResultRepository evaluationResultRepository, 
        IReviewRepository reviewRepository)
    {
        _logger = logger;
        _evaluationResultRepository = evaluationResultRepository;
        _reviewRepository = reviewRepository;
    }

    public async Task<Result<EvaluationResultEntity, Errors>> Execute(RecalculateEvaluationResultRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _evaluationResultRepository.GetByIdAsync(request.ResultId);

        if (result is null)
            return GeneralErrors.NotFound(request.ResultId).ToErrors();
        
        var reviews = await _reviewRepository.GetByReviewee(result.UserId, result.ProjectId, result.PeriodId);
        
        result.Recalculate(reviews);
        
        await _evaluationResultRepository.SaveAsync(cancellationToken);
        
        _logger.LogInformation("инфа");
        
        return result;
    }
}