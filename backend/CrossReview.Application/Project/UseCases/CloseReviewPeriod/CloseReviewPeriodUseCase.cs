using CrossReview.Application.Review;
using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.CloseReviewPeriod;

public class CloseReviewPeriodUseCase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IReviewRepository _reviewRepository;
    
    public CloseReviewPeriodUseCase(IProjectRepository projectRepository, IReviewRepository reviewRepository)
    {
        _projectRepository = projectRepository;
        _reviewRepository = reviewRepository;
    }

    public async Task<UnitResult<Errors>> Execute(CloseReviewPeriodRequest request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
            return GeneralErrors.NotFound(request.ProjectId).ToErrors();

        var reviewPeriod = project.ReviewPeriods
            .FirstOrDefault(rp => rp.Id == request.ReviewPeriodId);

        if (reviewPeriod is null)
            return GeneralErrors.NotFound(request.ReviewPeriodId).ToErrors();

        reviewPeriod.Close();

        // Закрываем все Submitted ревью для этого периода
        var reviews = await _reviewRepository.GetByProject(
            request.ProjectId, null, null, request.ReviewPeriodId, cancellationToken);
        
        foreach (var review in reviews.Where(r => r.Status == EnumReviewStatus.Submitted))
        {
            review.Close();
        }

        await _projectRepository.SaveAsync(cancellationToken);
        await _reviewRepository.SaveAsync(cancellationToken);

        return Result.Success<Errors>();
    }
}