using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Domain.Project;

public class ProjectEntity
{
    private List<ProjectMember> _members;
    private List<ReviewPeriod> _periods;

    private ProjectEntity(
        Guid id, 
        string title, 
        string description)
    {
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

    public static ProjectEntity Create(string title, string description)
    {
        return new ProjectEntity(Guid.NewGuid(), title, description);
    }
    
    public UnitResult<Errors> ToActivate()
    {
        if (Status)
            return GeneralErrors.ValueIsInvalid().ToErrors();// как-нибудь сообщить что статус и так уже true ?

        if (!_members.Any())
            return GeneralErrors.CollectionEmpty().ToErrors();

        if (!_periods.Any())
            return  GeneralErrors.CollectionEmpty().ToErrors();
        
        Status = true;

        return UnitResult.Success<Errors>();
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
    
    public Guid AssignEmployeeToProject(Guid? userId, EnumProjectRole role)
    {
        ProjectMember member = _members.Find(m => m.UserId == userId);

        if (member is null && userId is not null)
            throw new ValidationException("Такого пользователя нет в проекте");
        
        if (member is null && userId is null)
        {
            member = ProjectMember.Create(role);
            _members.Add(member);
            
            return member.UserId;
        }

        if (member.IsActive)
            throw new ValidationException("Пользователь уже имеет статус активного");
        
        member.ReturnToProject();
        
        return member.UserId;
    }

    public UnitResult<Error> RemoveEmployeeFromProject(Guid userId)
    {
        ProjectMember member = _members.Find(m => m.UserId == userId);

        if (member is null)
            return GeneralErrors.NotFound(userId);
        
        member.LeaveTheProject();
        _members.Remove(member);
        
        return UnitResult.Success<Error>();
    }

    public UnitResult<Error> DeactivateEmployeeInProject(Guid userId)
    {
        ProjectMember member = _members.Find(m => m.UserId == userId);

        if (member is null)
            return GeneralErrors.NotFound(userId);
        
        member.StopActivity();

        return UnitResult.Success<Error>();
    }

    public UnitResult<Errors> ChangeEmployeeRole(Guid projectMemberId, EnumProjectRole newRole)
    {
        ProjectMember member = _members.Find(m => m.UserId == projectMemberId);

        if (member is null)
            return GeneralErrors.NotFound(projectMemberId).ToErrors();
        
        member.ChangeRole(newRole);
        
        return UnitResult.Success<Errors>();
    }

    public Guid CreateReviewPeriod(DateTime startDate, DateTime endDate)
    {
        if (startDate >= endDate)
            throw new ValidationException(
                $"Значение поля {nameof(startDate)} не может быть позже или равно полю {nameof(endDate)}");
        
        var period = ReviewPeriod.Create(startDate, endDate);
        
        _periods.Add(period);

        return period.Id;
    }
    
    public void ActivateReviewPeriod(Guid periodId)
    {
        var period = _periods.Find(p => p.Id == periodId);
        
        if (period is null)
            throw new ValidationException("Нельзя сменить статус на активный у пустого периода");

        if (period.Status == EnumReviewPeriodStatus.Active &&
            _periods.Any(p => p.Status == EnumReviewPeriodStatus.Active))
            throw new ValidationException("Нельзя добавить еще один активный период");
        
        period.Activate();
    }
}