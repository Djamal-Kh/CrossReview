using CrossReview.Domain.Review;

namespace CrossReview.Application.Review.DTOs;

public record ReviewDto
{
    public Guid Id { get; set; }

    public Guid ReviewerId { get; set; }
    public Guid RevieweeId { get; set; }

    public Guid ProjectId { get; set; }
    public Guid TemplateId { get; set; }
    public Guid PeriodId { get; set; }

    public EnumReviewStatus Status { get; set; }

    public List<ReviewAnswerDto> Answers { get; set; }
}