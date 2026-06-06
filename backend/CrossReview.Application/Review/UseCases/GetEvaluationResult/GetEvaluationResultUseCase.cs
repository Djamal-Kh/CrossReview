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

    public async Task<Result<List<EvaluationResultDto>, Errors>> Execute(GetEvaluationResultRequest request,
        CancellationToken cancellationToken)
    {
        var evResults = await _evaluationResultRepository
            .GetByUserIdAsync(request.UserId, cancellationToken);
        
        var resultDtos = evResults.Select(evResult => new EvaluationResultDto
        {
            Id = evResult.Id,
            UserId = evResult.UserId,
            ProjectId = evResult.ProjectId,
            PeriodId = evResult.PeriodId,
            FinalScore = evResult.FinalScore,
            CalculatedAt = evResult.CalculatedAt,
        }).ToList();
        
        return resultDtos;
    }
}