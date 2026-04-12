namespace CrossReview.Domain.Review;

public class Review
{
    private List<ReviewAnswer> _answers;

    public Review(
        Guid id, 
        Guid reviewerId, 
        Guid revieweeId, 
        Guid projectId, 
        Guid templateId, 
        IEnumerable<ReviewAnswer> answers, 
        Guid periodId)
    {
        Id = id;
        ReviewerId = reviewerId;
        RevieweeId = revieweeId;
        ProjectId = projectId;
        TemplateId = templateId;
        _answers = answers.ToList();
        PeriodId = periodId;
    }
    
    public Guid Id { get; }
    public Guid ReviewerId { get; } 
    public Guid RevieweeId { get; } 
    public Guid ProjectId { get; }
    public Guid TemplateId { get; }
    public IReadOnlyList<ReviewAnswer> Answers => _answers;
    public Guid PeriodId { get; } 
}