using System.ComponentModel.DataAnnotations;

namespace CrossReview.Domain.Project;

public class Project
{
    private List<ProjectMember> _members;
    private List<ReviewPeriod> _periods;

    public Project(
        Guid id, 
        string title, 
        string description)
    {
        Validate(title);
        
        Id = id;
        Title = title;
        Status = false;
        Description = description;
        _members = [];
        _periods = [];
    }
    
    public Guid Id { get; }
    public string Title { get; private set; }
    public bool Status { get; private set; }
    public string? Description { get; private set; }
    public IReadOnlyCollection<ProjectMember> Members => _members;
    public IReadOnlyCollection<ReviewPeriod> Periods => _periods;


    public void Validate(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException($"Поле {Title} не может быть пустым");
    }
    
    public void AddEmployeeToProject(Guid userId, EnumProjectRole role)
    {
        ProjectMember member = _members.Find(m => m.UserId == userId);

        if (member is null)
        {
            _members.Add(new ProjectMember(userId, role));
            return;
        }

        if (member.IsActive)
            throw new ValidationException("Пользователь уже имеет статус активного в проекте");
        
        member.ReturnToProject();
    }

    public void RemoveEmployeeFromProject(Guid userId)
    {
        ProjectMember member = _members.Find(m => m.UserId == userId);

        if (member is null)
            throw new ValidationException("Такого пользователя нет в этом проекте");
        
        member.LeaveTheProject();
        _members.Remove(member);
    }

    public void StopActivityEmployeeInProject(Guid userId)
    {
        ProjectMember member = _members.Find(m => m.UserId == userId);
        
        if (member is null)
            throw new ValidationException("Такого пользователя нет в этом проекте");
        
        member.StopActivity();
    }
    
    public void AddNewReviewPeriod(ReviewPeriod reviewPeriod)
    {
        if (reviewPeriod is null)
            throw new ValidationException("Нельзя добавить пустой период");

        if (_periods.Any(x => x.Id == reviewPeriod.Id))
            throw new ValidationException("Такой период уже существует");

        if (reviewPeriod.Status == EnumReviewPeriodStatus.Active &&
            _periods.Any(p => p.Status == EnumReviewPeriodStatus.Active))
            throw new ValidationException("Нельзя добавить еще один активный период");
        
        _periods.Add(reviewPeriod);
    }

    public void ToActivate()
    {
        if (Status)
            return; // как-нибудь сообщить что статус и так уже false ?

        if (!_members.Any())
            throw new ValidationException("Перед тем как сделать статус активным добавьте к проекту сотрудников");

        if (!_periods.Any())
            throw new ValidationException("Перед тем как сделать статус активным добавьте к проекту период ревью");
        
        Status = true;
    }

    public void ToDeactivate()
    {
        if(Status is false)
            return; // как-нибудь сообщить что статус и так уже false ?
        
        Status = false;
    }

    public void UpdateTitle(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle))
            throw new ValidationException($"Поле {Title} не может быть пустым");
        
        Title = newTitle;
    }

    public void UpdateDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            throw new ValidationException($"Попытка добавить пустое значение");
        
        Description = newDescription;
    }

    public void UpdateData(string title, string description)
    {
        UpdateTitle(title);
        UpdateDescription(description);
    }
}