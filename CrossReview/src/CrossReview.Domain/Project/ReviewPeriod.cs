namespace CrossReview.Domain.Project;

public class ReviewPeriod
{
    public ReviewPeriod(
        Guid id, 
        DateTime startDate,
        DateTime endDate, 
        EnumReviewPeriodStatus status = EnumReviewPeriodStatus.Closed)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
    }
    
    public Guid Id { get; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public EnumReviewPeriodStatus Status { get; private set; }
}