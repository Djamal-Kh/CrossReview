using CrossReview.Domain.Review.ValueObjects;

namespace CrossReview.Domain.Review;

public class Review
{
    public Guid Id { get; set; }
    public Guid ReviewerId { get; set; } // ссылка на id ProjectMember ?
    public Guid RevieweeId { get; set; } // ссылка на id ProjectMember >
    public Guid ProjectId { get; set; }
    public Guid TemplateId { get; set; }
    public List<ReviewAnswer> Answers { get; set; }
    public Guid PeriodId { get; set; } // ссылка на ReviewPeriod
}