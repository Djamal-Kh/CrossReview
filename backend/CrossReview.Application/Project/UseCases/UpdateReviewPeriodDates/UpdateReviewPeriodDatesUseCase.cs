using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.UpdateReviewPeriodDates;

public class UpdateReviewPeriodDatesUseCase
{
    private readonly IProjectRepository _projectRepository;
    
    public UpdateReviewPeriodDatesUseCase(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<UnitResult<Errors>> Execute(UpdateReviewPeriodDatesRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        var reviewPeriod = project.ReviewPeriods.FirstOrDefault(rp => rp.Id == request.ReviewPeriodId);

        if (reviewPeriod is null)
            return GeneralErrors.NotFound(request.ReviewPeriodId).ToErrors();

        reviewPeriod.UpdateDates(request.StartDate, request.EndDate);
        
        await _projectRepository.SaveAsync(cancellationToken);

        return Result.Success<Errors>();
    }
}