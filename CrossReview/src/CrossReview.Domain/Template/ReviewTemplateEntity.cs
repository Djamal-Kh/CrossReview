using System.ComponentModel.DataAnnotations;

namespace CrossReview.Domain.Template;

public class ReviewTemplateEntity
{
    private List<ReviewQuestion> _questions;
    
    private const int MaxTitleLenght = 300;
    private const int MinTitleLenght = 10;
    
    public ReviewTemplateEntity(
        Guid id, 
        string title, 
        IEnumerable<ReviewQuestion> questions, 
        bool isActive = false)
    {
        Validate(id, title);
        
        Id = id;
        Title = title;
        _questions = new List<ReviewQuestion>();
        
        foreach (var q in questions)
        {
            AddQuestion(q);
        }
        
        IsActive = isActive;
    }
    public Guid Id { get; }
    public string Title { get; private set; }
    public IReadOnlyList<ReviewQuestion> Questions => _questions;
    public bool IsActive { get; private set; }

    
    public void AddQuestion(ReviewQuestion question)
    {
        EnsureEditable();
        
        if (_questions.Any(q => q.Title == question.Title))
            throw new ValidationException("Такой вопрос уже есть в списке вопросов !");
        
        _questions.Add(question);
    }

    public void UpdateQuestion(Guid questionId, string newTitle, double newWeight)
    {
        EnsureEditable();

        if (!_questions.Any())
            throw new ValidationException("В шаблоне нет ни одного вопроса");
        
        var question = _questions.FirstOrDefault(t => t.Id == questionId);

        if (question is null)
            throw new ValidationException("Вопрос не найден");
        
        question.Update(newTitle, newWeight);
    }
    
    public void RemoveQuestion(Guid questionId)
    {
        EnsureEditable();
        
        if (!_questions.Any())
            throw new ValidationException("В шаблоне нет ни одного вопроса");
        
        var question = _questions.FirstOrDefault(t => t.Id == questionId);

        if (question is null)
            throw new ValidationException("Вопрос не найден");
        
        _questions.Remove(question);
    }

    public void ValidateWeight()
    {
        const double targetValue = 1.0; 
        
        var sum = _questions.Sum(q => q.Weight);

        if (Math.Abs(sum - targetValue) > 0.001)
            throw new ValidationException($"Общая весомость должна быть равна {targetValue}");
    }
    
    private void EnsureEditable()
    {
        if (IsActive)
            throw new ValidationException("Шаблон нельзя изменять");
    }
    
    public void Activate()
    {
        if (IsActive)
            throw new ValidationException("Шаблон и так имеет статус активного");

        if (!_questions.Any())
            throw new ValidationException("Нельзя изменить статус шаблона на активный без вопросов");
            
        ValidateWeight();
        
        IsActive = true;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new ValidationException("Шаблон и так имеет статус неактивного");
        
        IsActive = false;
    }
    
    private void Validate(Guid id, string title)
    {
        if (id == Guid.Empty)
            throw new ValidationException($"Поле {nameof(Id)} не может быть пустым");
        
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException("Заголовок вопроса не может быть пустым");

        if (title.Length > MaxTitleLenght)
            throw new ValidationException($"Заголовок вопроса должен содержать не более {MaxTitleLenght} символов");
        
        if (title.Length < MinTitleLenght)
            throw new ValidationException($"Заголовок вопроса должен содержать не менее {MinTitleLenght} символов");
    }
}