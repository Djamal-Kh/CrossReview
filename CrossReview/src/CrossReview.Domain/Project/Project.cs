using CrossReview.Domain.Project.ValueObjects;
using CrossReview.Domain.Review.ValueObjects;

namespace CrossReview.Domain.Project;

public class Project
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; } = string.Empty;
    public List<ProjectMember> Members { get; set; } = [];
    public List<ReviewPeriod> Period { get; set; }
    // нужна ли связь с Review ? 
}