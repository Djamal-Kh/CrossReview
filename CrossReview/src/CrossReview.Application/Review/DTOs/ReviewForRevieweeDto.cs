using CrossReview.Domain.Review;

namespace CrossReview.Application.Review.DTOs;

public record ReviewForRevieweeDto()
{
    public Guid Id { get; init; }
    public EnumReviewStatus Status { get; init; }
    public IReadOnlyCollection<ReviewAnswer> Answers { get; init; }
}