namespace CrossReview.Application.Review.DTOs;

public record ReviewDashboardDto
{
    public Guid ProjectId { get; set; }
    public Guid PeriodId { get; set; }

    public List<ReviewDto> Reviews { get; set; }
    public List<EvaluationResultDto> Results { get; set; }
}