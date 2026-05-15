namespace CrossReview.Application.Review.DTOs;

public record ReviewAnswerDto
{
    public Guid QuestionId { get; set; }
    public int Score { get; set; }
    public string? Comment { get; set; }
}