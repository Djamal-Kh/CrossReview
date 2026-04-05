namespace CrossReview.Domain.Review;

public class EvaluationResult
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProjectId { get; set; }
    public int PeriodId { get; set; }
    public int FinalScore { get; set; }
    public DateTime CalculatedAt { get; set; } // Вынести отсюда ?
}