namespace CrossReview.Domain.Review;

public class EvaluationResult
{
    public EvaluationResult(
        Guid id,
        Guid userId,
        Guid projectId,
        Guid periodId)
    {
        Id = id;
        UserId = userId;
        ProjectId = projectId;
        PeriodId = periodId;
        CalculatedAt = DateTime.UtcNow;
    }
    
    public Guid Id { get; }
    public Guid UserId { get; }
    public Guid ProjectId { get; }
    public Guid PeriodId { get; }
    public int FinalScore { get; private set; }
    public DateTime CalculatedAt { get; } 
}