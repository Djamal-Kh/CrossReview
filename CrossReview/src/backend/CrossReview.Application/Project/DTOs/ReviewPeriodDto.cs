using CrossReview.Domain.Project;

namespace CrossReview.Application.Project.DTOs;

public record ReviewPeriodDto
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public EnumReviewPeriodStatus Status { get; set; }
}