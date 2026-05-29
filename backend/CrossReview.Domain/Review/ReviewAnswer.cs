using System.ComponentModel.DataAnnotations;

namespace CrossReview.Domain.Review;

public class ReviewAnswer
{
    private ReviewAnswer(Guid reviewId, Guid questionId, int score, string comment)
    {
        if (questionId == Guid.Empty)
            throw new ValidationException($"Поле {nameof(QuestionId)} не может быть пустым");
        
        Validate(score, comment);

        ReviewId = reviewId;
        QuestionId = questionId;
        Score = score;
        Comment = comment;
    }
    
    // для ef core
    private ReviewAnswer () {} 
    
    // todo реализовать анонимность на уровне БД. Для этого убрать поле ReviewId или зашифровать его ?
    
    public Guid QuestionId { get; }
    public Guid ReviewId { get; private set; }
    public int Score { get; private set; }
    public string? Comment { get; private set; }

    public static ReviewAnswer Create(Guid reviewId, Guid questionId,int score, string comment)
    {
        return new ReviewAnswer(reviewId, questionId, score, comment);
    }
    
    public void Update(int newScore, string newComment)
    {
        Validate(newScore, newComment);
        
        Score = newScore;
        Comment = newComment;
    }

    // а надо ?
    public void ClearComment()
    {
        Comment = string.Empty;
    }
    
    private void Validate(int score, string comment)
    {
        if (comment.Length > 1000)
            throw new ValidationException("Комментарий должен содержать не более 1000 символов");

        if (score > 10 || score < 1)
            throw new ValidationException("Неверная оценка");
    }
}