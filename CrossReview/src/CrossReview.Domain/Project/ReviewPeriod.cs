namespace CrossReview.Domain.Review.ValueObjects;

public class ReviewPeriod
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public EnumReviewPeriodStatus Status { get; set; }
}