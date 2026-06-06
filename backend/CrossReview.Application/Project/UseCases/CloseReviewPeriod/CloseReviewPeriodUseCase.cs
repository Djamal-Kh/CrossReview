using CrossReview.Application.Review;
using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Project.UseCases.CloseReviewPeriod;

public class CloseReviewPeriodUseCase
{
    private readonly IProjectRepository _projectRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IEvaluationResultRepository _evaluationResultRepository;
    
    public CloseReviewPeriodUseCase(
        IProjectRepository projectRepository, 
        IReviewRepository reviewRepository, 
        IEvaluationResultRepository evaluationResultRepository)
    {
        _projectRepository = projectRepository;
        _reviewRepository = reviewRepository;
        _evaluationResultRepository = evaluationResultRepository;
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

        var revieweeIds = reviews
            .Where(r => r.Status == EnumReviewStatus.Closed)
            .Select(r => r.RevieweeId)
            .Distinct();

        foreach (var revieweeId in revieweeIds)
        {
            var existing = await _evaluationResultRepository
                .GetByParametersAsync(revieweeId, request.ProjectId, request.ReviewPeriodId, cancellationToken);
            
            if (existing is not null)
            {
                existing.Recalculate(reviews
                    .Where(r => r.RevieweeId == revieweeId)
                    .Cast<ReviewEntity?>()
                    .ToList());
            }
            else
            {
                var result = EvaluationResultEntity.Create(
                    revieweeId, request.ProjectId, request.ReviewPeriodId);
                result.Calculate(reviews
                    .Where(r => r.RevieweeId == revieweeId)
                    .ToList());
                await _evaluationResultRepository.AddAsync(result, cancellationToken);
            }
        }
        
        await _projectRepository.SaveAsync(cancellationToken);
        await _reviewRepository.SaveAsync(cancellationToken);
        await _evaluationResultRepository.SaveAsync(cancellationToken);

        return Result.Success<Errors>();
    }
}