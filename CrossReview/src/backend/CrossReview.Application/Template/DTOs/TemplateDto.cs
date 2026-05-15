namespace CrossReview.Application.Template.DTOs;

public record TemplateDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }

    public string Title { get; set; }
    public bool IsActive { get; set; }

    public List<ReviewQuestionDto> Questions { get; set; }
}