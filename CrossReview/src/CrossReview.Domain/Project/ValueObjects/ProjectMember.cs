namespace CrossReview.Domain.Project.ValueObjects;

public class ProjectMember
{
    public Guid UserId { get; set; }
    public EnumProjectRole Role { get; set; }
    public DateTime JoinedAt { get; set; } // вынести отсюда ?
    public DateTime? LeftAt { get; set; }  // вынести отсюда ? 
}