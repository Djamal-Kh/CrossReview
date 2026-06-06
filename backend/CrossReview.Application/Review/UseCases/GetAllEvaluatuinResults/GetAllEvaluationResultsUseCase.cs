using CrossReview.Application.Review.DTOs;
using CSharpFunctionalExtensions;

namespace CrossReview.Application.Review.UseCases.GetAllEvaluatuinResults;

public class GetAllEvaluationResultsUseCase
{
    private readonly IEvaluationResultRepository _evaluationResultRepository;
    
    public GetAllEvaluationResultsUseCase(IEvaluationResultRepository evaluationResultRepository)
    {
        _evaluationResultRepository = evaluationResultRepository;
    }

    public async Task<List<EvaluationResultDto>> Execute(CancellationToken cancellationToken)
    {
        var results = await _evaluationResultRepository.GetAllAsync(cancellationToken);

        var dtos = results.Select(r => new EvaluationResultDto
        {
            Id = r.Id,
            UserId = r.UserId,
            ProjectId = r.ProjectId,
            PeriodId = r.PeriodId,
            FinalScore = r.FinalScore,
            CalculatedAt = r.CalculatedAt,
        }).ToList();
        
        return dtos;
    }
}