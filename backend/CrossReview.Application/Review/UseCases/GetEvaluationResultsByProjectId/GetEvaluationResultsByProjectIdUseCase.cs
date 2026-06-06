using CrossReview.Application.Project;
using CrossReview.Application.Review.DTOs;
using CrossReview.Domain.Project;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.GetEvaluationResultsByProjectId;

public class GetEvaluationResultsByProjectIdUseCase
{
    private readonly IEvaluationResultRepository _evaluationResultRepository;
    private readonly IProjectRepository _projectRepository;
    
    public GetEvaluationResultsByProjectIdUseCase(IEvaluationResultRepository evaluationResultRepository, IProjectRepository projectRepository)
    {
        _evaluationResultRepository = evaluationResultRepository;
        _projectRepository = projectRepository;
    }

    public async Task<Result<List<EvaluationResultDto>, Errors>> Execute(GetEvaluationResultsByProjectIdRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        var isTeamLead = project.Members
            .Any(m => m.UserId == request.UserId && m.Role == EnumProjectRole.TeamLead && m.IsActive);

        if (!request.IsAdmin && !isTeamLead)
            return GeneralErrors
                .Failure("текущий пользователь не является администратором или руководителем")
                .ToErrors();

        var results = await _evaluationResultRepository.GetAllAsync(cancellationToken);

        var filtered = results.Where(r => r.ProjectId == request.ProjectId);
        
        var dtos = filtered.Select(r => new EvaluationResultDto
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