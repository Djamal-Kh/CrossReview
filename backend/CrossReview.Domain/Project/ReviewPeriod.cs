using System.ComponentModel.DataAnnotations;

namespace CrossReview.Domain.Project;

public class ReviewPeriod
{
    private ReviewPeriod(
        Guid id, 
        Guid projectId,
        DateTime startDate,
        DateTime endDate, 
        EnumReviewPeriodStatus status = EnumReviewPeriodStatus.Draft)
    {
        Validate(id, startDate, endDate);
        
        Id = id;
        ProjectId = projectId;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
    }
    
    // для ef core
    private ReviewPeriod () {}
    
    public Guid Id { get; }
    public Guid ProjectId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public EnumReviewPeriodStatus Status { get; private set; }

    public static ReviewPeriod Create(Guid projectId, DateTime startDate, DateTime endDate)
    {
        return new ReviewPeriod(Guid.NewGuid(), projectId, startDate, endDate);
    }
    
    public bool IsActiveNow()
    {
        var currentTime = DateTime.UtcNow;
        
        if (Status is EnumReviewPeriodStatus.Active && currentTime >= StartDate && currentTime <= EndDate)
            return true;

        return false;
    }
    
    public void Activate()
    {
        if (Status is EnumReviewPeriodStatus.Active)
            return; // как-нибудь сообщить что статус и так уже Active ?
        
        if (Status is not EnumReviewPeriodStatus.Draft)
            throw new ValidationException($"Можно запустить ревью только со статусом {nameof(EnumReviewPeriodStatus.Draft)}");
        
        var currentTime = DateTime.UtcNow;

        if (currentTime < StartDate)
            throw new ValidationException("Нельзя запустить ревью раньше времени");
            
        Status = EnumReviewPeriodStatus.Active;
    }

    public void Close()
    {
        if(Status is EnumReviewPeriodStatus.Closed)
            return; // как-нибудь сообщить что статус и так уже Closed ?

        if (Status is not EnumReviewPeriodStatus.Active)
            throw new ValidationException("Закрыть можно только активный ревью");
        
        Status = EnumReviewPeriodStatus.Closed;
    }

    public void Archive()
    {
        if (Status is EnumReviewPeriodStatus.Archive)
            return;

        if (Status is not EnumReviewPeriodStatus.Closed)
            throw new ValidationException("Архивировать можно только закрытые ревью");
        
        var currentTime = DateTime.UtcNow;

        if (currentTime <= EndDate)
            throw new ValidationException("Нельзя архивировать ревью до его окончания");
        
        Status = EnumReviewPeriodStatus.Archive;
    }
    
    public void UpdateDates(DateTime startDate, DateTime endDate)
    {
        if (Status is EnumReviewPeriodStatus.Archive)
            throw new ValidationException("Данный период находится в архиве");

        if (Status is EnumReviewPeriodStatus.Active)
            throw new ValidationException("Нельзя остановить текущее ревью"); 
            // принудительно остановить ревью может только администратор
        
        if (startDate == default || endDate == default)
            throw new ValidationException("Дата не инициализирована");
        
        if (startDate >= endDate)
            throw new ValidationException(
                $"Значение поля {nameof(startDate)} не может быть позже или равно полю {nameof(endDate)}");
        
        StartDate = startDate;
        EndDate = endDate;
    }
    
    private void Validate(Guid id, DateTime startDate, DateTime endDate)
    {
        if (id == Guid.Empty)
            throw new ValidationException($"Поле {nameof(Id)} не должно быть пустым");

        if (startDate == default || endDate == default)
            throw new ValidationException("Дата не инициализирована");
        
        if (startDate >= endDate)
            throw new ValidationException("Ревью не может закончиться до начала ревью");
    }
}