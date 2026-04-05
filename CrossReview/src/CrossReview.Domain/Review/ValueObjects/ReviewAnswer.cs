namespace CrossReview.Domain.Review.ValueObjects;

public class ReviewAnswer
{
    public Guid QuestionId { get; set; }
    public int Score { get; set; }
    public string? Comment { get; set; } = string.Empty;
}