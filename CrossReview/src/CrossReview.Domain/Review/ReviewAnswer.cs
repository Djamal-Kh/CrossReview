namespace CrossReview.Domain.Review;

public class ReviewAnswer
{
    public ReviewAnswer(Guid questionId, int score, string comment)
    {
        QuestionId = questionId;
        Score = score;
        Comment = comment;
    }
    
    public Guid QuestionId { get;  }
    public int Score { get; }
    public string? Comment { get; }
}