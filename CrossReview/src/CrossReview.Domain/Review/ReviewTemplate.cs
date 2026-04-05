using CrossReview.Domain.Review.ValueObjects;

namespace CrossReview.Domain.Review;

public class ReviewTemplate
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public List<ReviewQuestion> Questions { get; set; }
    public bool IsActive { get; set; } = true;
}