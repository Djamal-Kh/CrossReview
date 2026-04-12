namespace CrossReview.Domain.Project;

public class ProjectMember
{
    public ProjectMember(Guid userId, EnumProjectRole role)
    {
        UserId = userId;
        Role = role;
        JoinedAt = DateTime.UtcNow;
        LeftAt = null;
    }
    
    public Guid UserId { get;}
    public EnumProjectRole Role { get; }
    public DateTime JoinedAt { get;} 
    public DateTime? LeftAt { get; private set; }

    public async Task RemoveFromProject()
    {
        LeftAt = DateTime.UtcNow;
    }
}