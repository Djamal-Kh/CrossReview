namespace CrossReview.Application.Template.DTOs;

public record ReviewQuestionDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public double Weight { get; set; }
}