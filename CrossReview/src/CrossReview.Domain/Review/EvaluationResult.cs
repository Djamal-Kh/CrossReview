using System.ComponentModel.DataAnnotations;

namespace CrossReview.Domain.Review;

public class EvaluationResult
{
    public EvaluationResult(
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
    public int FinalScore { get; private set; }
    public DateTime CalculatedAt { get; private set; }

    public void Calculate(IEnumerable<ReviewEntity> reviews)
    {
        if (!reviews.Any())
            throw new ValidationException("Не найдено ни одного ревью");

        var completedReviews = reviews
            .Where(r => r.Status == EnumReviewStatus.Submitted)
            .ToList();

        if (!completedReviews.Any())
            throw new ValidationException("Не найдено ни одного опубликованного ревью");

        var scores = completedReviews
            .Select(s => s.CalculateAverageScore())
            .ToList();

        FinalScore = (int)Math.Round(scores.Average());
        CalculatedAt = DateTime.UtcNow;
    }

    public void Recalculate(IEnumerable<ReviewEntity> reviews)
    {
        if (CalculatedAt == default)
            throw new ValidationException("Ревью не были ни разу подсчитаны");
        
        Calculate(reviews);
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