using System.ComponentModel.DataAnnotations;

namespace CrossReview.Domain.Project;

public class ProjectMember
{
    private ProjectMember(Guid userId, Guid projectId, EnumProjectRole role)
    {
        if (userId == Guid.Empty)
            throw new ValidationException($"Поле {UserId} не может быть пустым");
        
        UserId = userId;
        ProjectId = projectId;
        Role = role;
        IsActive = true;
        JoinedAt = DateTime.UtcNow;
        LeftAt = null;
    }
    
    public Guid UserId { get; }
    public Guid ProjectId { get; private set; }
    public EnumProjectRole Role { get; private set; }
    public bool IsActive {get; private set;}
    public DateTime JoinedAt { get; private set; } 
    public DateTime? LeftAt { get; private set; }

    public static ProjectMember Create(Guid projectId, EnumProjectRole role)
    {
        return new ProjectMember(Guid.NewGuid(), projectId, role);
    }
    
    public void ChangeRole(EnumProjectRole newRole)
    {
        if (Role == newRole)
            return;

        Role = newRole;
    }

    public void LeaveTheProject()
    {
        LeftAt = DateTime.UtcNow;
    }

    public void StopActivity()
    {
        IsActive = false;
    }
    
    public void ReturnToProject()
    {
        if (IsActive)
            throw new ValidationException("Пользователь уже имеет статус активного в проекте");
        
        LeftAt = null;
    }
}