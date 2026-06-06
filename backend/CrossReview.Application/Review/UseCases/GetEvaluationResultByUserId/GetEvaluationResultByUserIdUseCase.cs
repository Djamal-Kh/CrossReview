using CrossReview.Application.Review.DTOs;

namespace CrossReview.Application.Review.UseCases.GetEvaluationResulyByUserId;

public class GetEvaluationResultByUserIdUseCase
{
    private readonly IEvaluationResultRepository _evaluationResultRepository;
    
    public GetEvaluationResultByUserIdUseCase(IEvaluationResultRepository evaluationResultRepository)
    {
        _evaluationResultRepository = evaluationResultRepository;
    }

    public async Task<List<EvaluationResultDto>> Execute(GetEvaluationResultByUserIdRequest request,
        CancellationToken cancellationToken)
    {
        var results = await _evaluationResultRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        
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