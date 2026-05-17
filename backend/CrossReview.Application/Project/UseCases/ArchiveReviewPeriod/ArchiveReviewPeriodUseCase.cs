using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.ArchiveReviewPeriod;

public class ArchiveReviewPeriodUseCase
{
    private readonly IProjectRepository _projectRepository;
    
    public ArchiveReviewPeriodUseCase(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<UnitResult<Errors>> Execute(ArchiveReviewPeriodRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        
        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        var reviewPeriod = project.ReviewPeriods.FirstOrDefault(rp => rp.Id == request.ReviewPeriodId);

        if (reviewPeriod is null)
            return GeneralErrors.NotFound(request.ReviewPeriodId).ToErrors();

        reviewPeriod.Archive();
        
        await _projectRepository.SaveAsync(cancellationToken);
        
        return Result.Success<Errors>();
    }
}