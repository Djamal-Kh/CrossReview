using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.CloseReviewPeriod;

public class CloseReviewPeriodUseCase
{
    private readonly IProjectRepository _projectRepository;
    
    public CloseReviewPeriodUseCase(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<UnitResult<Errors>> Execute(CloseReviewPeriodRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        var reviewPeriod = project.ReviewPeriods.FirstOrDefault(rp => rp.Id == request.ReviewPeriodId);

        if (reviewPeriod is null)
            return GeneralErrors.NotFound(request.ReviewPeriodId).ToErrors();
        
        reviewPeriod.Close();

        await _projectRepository.SaveAsync(cancellationToken);

        return Result.Success<Errors>();
    }
}