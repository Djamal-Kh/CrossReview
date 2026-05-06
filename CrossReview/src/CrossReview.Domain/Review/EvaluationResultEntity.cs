using System.ComponentModel.DataAnnotations;

namespace CrossReview.Domain.Review;

public class EvaluationResultEntity
{
    private EvaluationResultEntity(
        Guid id,
        Guid userId,
        Guid projectId,
        Guid periodId)
    {
        Validate(id, userId, projectId, periodId);
        
        Id = id;
        UserId = userId;
        ProjectId = projectId;
        PeriodId = periodId;
    }
    
    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid ProjectId { get; }
    public Guid PeriodId { get; }
    public double FinalScore { get; private set; }
    public DateTime CalculatedAt { get; private set; }

    public static EvaluationResultEntity Create(Guid userId, Guid projectId, Guid periodId)
    {
        return new EvaluationResultEntity(Guid.NewGuid(), userId, projectId, periodId);
    }
    
    public void Calculate(List<ReviewEntity> reviews, bool recalculate = false)
    {
        if (!reviews.Any())
            throw new ValidationException("Результат уже был рассчитан");

        if (CalculatedAt != default && !recalculate)
            throw new ValidationException("Не найдено ни одного ревью");
        
        var completedReviews = reviews
            .Where(r => r.Status == EnumReviewStatus.Submitted)
            .ToList();

        if (!completedReviews.Any())
            throw new ValidationException("Не найдено ни одного опубликованного ревью");

        var scores = completedReviews
            .Select(s => s.CalculateAverageScore())
            .ToList();

        FinalScore = Math.Round(scores.Average(), 2);
        CalculatedAt = DateTime.UtcNow;
    }

    public void Recalculate(List<ReviewEntity?> reviews)
    {
        if (CalculatedAt == default)
            throw new ValidationException("Ревью не были ни разу подсчитаны");

        bool recalculate = true;
        
        Calculate(reviews, recalculate);
    }
    
    private void Validate(Guid id, Guid userId, Guid projectId, Guid periodId)
    {
        if (id == Guid.Empty)
            throw new ValidationException($"Поле {nameof(Id)} не должно быть пустым");

        if (userId == Guid.Empty)
            throw new ValidationException($"Поле {nameof(UserId)} не должно быть пустым");

        if (projectId == Guid.Empty)
            throw new ValidationException($"Поле {nameof(ProjectId)} не должно быть пустым");

        if (periodId == Guid.Empty)
            throw new ValidationException($"Поле {nameof(PeriodId)} не должно быть пустым");
    }
}