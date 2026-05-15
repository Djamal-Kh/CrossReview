using CrossReview.Domain.Project;

namespace CrossReview.Application.Project.DTOs;

public record ProjectMemberDto
{
    public Guid UserId { get; set; }
    public EnumProjectRole Role { get; set; }
    public bool IsActive { get; set; }

    public DateTime JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
}