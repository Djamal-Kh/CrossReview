namespace CrossReview.Application.Review.DTOs;

public record EvaluationResultDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public Guid PeriodId { get; set; }

    public double FinalScore { get; set; }
    public DateTime CalculatedAt { get; set; }
}