namespace CrossReview.Domain.Review.ValueObjects;

public class ReviewQuestion
{
    public Guid Id { get; set; }
    public string Text {get; set;} = string.Empty;
    public int Weight { get; set; }
}