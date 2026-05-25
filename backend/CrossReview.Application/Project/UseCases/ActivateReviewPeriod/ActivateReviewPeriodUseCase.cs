using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.ActivateReviewPeriod;

public class ActivateReviewPeriodUseCase
{
    private readonly ILogger<ActivateReviewPeriodUseCase> _logger;
    private readonly IProjectRepository _projectRepository;
    
    public ActivateReviewPeriodUseCase(
        ILogger<ActivateReviewPeriodUseCase> logger,
        IProjectRepository projectRepository)
    {
        _logger = logger;
        _projectRepository = projectRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(ActivateReviewPeriodRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();
        
        project.ActivateReviewPeriod(request.PeriodId);

        await _projectRepository.SaveAsync(cancellationToken);
        
        _logger.LogInformation("ReviewPeriod with Id: {Id} was activated", request.PeriodId);
        
        return request.PeriodId;
    }
}