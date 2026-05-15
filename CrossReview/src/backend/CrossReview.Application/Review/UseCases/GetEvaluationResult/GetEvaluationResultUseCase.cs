using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.GetEvaluationResult;

public class GetEvaluationResultUseCase
{
    private readonly ILogger<GetEvaluationResultUseCase> _logger;
    private readonly IEvaluationResultRepository _evaluationResultRepository;
    
    public GetEvaluationResultUseCase(
        ILogger<GetEvaluationResultUseCase> logger, 
        IEvaluationResultRepository evaluationResultRepository)
    {
        _logger = logger;
        _evaluationResultRepository = evaluationResultRepository;
    }

    public async Task<Result<EvaluationResultEntity, Errors>> Execute(GetEvaluationResultRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _evaluationResultRepository
            .GetByParametersAsync(request.UserId, request.ProjectId, request.PeriodId, cancellationToken);

        if (result is null)
            return GeneralErrors.NotFound(request.UserId).ToErrors();
        
        return result;
    }
}