using CrossReview.Application.Review.DTOs;
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

    public async Task<Result<EvaluationResultDto, Errors>> Execute(GetEvaluationResultRequest request,
        CancellationToken cancellationToken)
    {
        var evResult = await _evaluationResultRepository
            .GetByParametersAsync(request.UserId, request.ProjectId, request.PeriodId, cancellationToken);

        if (evResult is null)
            return GeneralErrors.NotFound(request.UserId).ToErrors();

        var result = new EvaluationResultDto
        {
            Id = evResult.Id,
            UserId = evResult.UserId,
            ProjectId = evResult.ProjectId,
            PeriodId =  evResult.PeriodId,
            FinalScore = evResult.FinalScore,
            CalculatedAt =  evResult.CalculatedAt,
        };
        
        return result;
    }
}