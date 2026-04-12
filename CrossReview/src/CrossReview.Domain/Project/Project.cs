namespace CrossReview.Domain.Project;

public class Project
{
    private List<ProjectMember> _members = [];
    private List<ReviewPeriod> _periods = [];

    public Project(
        Guid id, 
        string title, 
        bool isActive, 
        string description, 
        IEnumerable<ProjectMember> members, 
        IEnumerable<ReviewPeriod> period)
    {
        Id = id;
        Title = title;
        IsActive = isActive;
        Description = description;
        _members = members.ToList();
        _periods = period.ToList();
    }
    
    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; } = string.Empty;
    public IReadOnlyList<ProjectMember> Members => _members;
    public IReadOnlyList<ReviewPeriod> Periods => _periods;
}