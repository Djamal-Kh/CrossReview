namespace CrossReview.Domain.Template;

public class ReviewQuestion
{
    public ReviewQuestion(Guid id, string text, int weight)
    {
        Id = id;
        Text = text;
        Weight = weight;
    }
    
    public Guid Id { get; }
    public string Text {get; } 
    public int Weight { get; }
}