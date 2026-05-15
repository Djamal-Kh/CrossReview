namespace CrossReview.Application.Project.DTOs;

public record ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool Status { get; set; }
    public string? Description { get; set; }

    public List<ProjectMemberDto> Members { get; set; }
    public List<ReviewPeriodDto> ReviewPeriods { get; set; }
}