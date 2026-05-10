using System.ComponentModel.DataAnnotations;

namespace CrossReview.Domain.Template;

public class ReviewQuestion
{
    private const int MaxTitleLenght = 300;
    private const int MinTitleLenght = 10;
    private const double MaxWeight = 1;
    private const double MinWeight = 0.1;
    
    private ReviewQuestion(Guid id, Guid templateId, string title, double weight)
    {
        ValidateTitle(title);
        ValidateWeight(weight);
        
        Id = id;
        Title = title;
        Weight = weight;
    }
    
    public Guid Id { get; }
    public string Title {get; private set; } 
    public double Weight { get; private set; }
    public Guid TemplateId { get; private set; }

    public static ReviewQuestion Create(Guid templateId,string title, double weight)
    {
        return new ReviewQuestion(Guid.NewGuid(), templateId, title, weight);
    }
    
    public void Update(string? newTitle, double newWeight = 0)
    {

        if (newWeight == 0 && newTitle is null)
            throw new Exception();
        
        if (newWeight == 0)
        {
            ValidateTitle(newTitle);
            Title = newTitle;
        }
        
        if (newTitle is null)
            ValidateWeight(newWeight);

        Title = newTitle;
        Weight = newWeight;
    }
    
    private void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ValidationException("Заголовок вопроса не может быть пустым");

        if (title.Length > MaxTitleLenght)
            throw new ValidationException($"Заголовок вопроса должен содержать не более {MaxTitleLenght} символов");
        
        if (title.Length < MinTitleLenght)
            throw new ValidationException($"Заголовок вопроса должен содержать не менее {MinTitleLenght} символов");
    }
    
    private void ValidateWeight(double weight)
    {
        if (weight > MaxWeight)
            throw new ValidationException($"Весомость вопроса не может превышать {MaxWeight}");
        
        if (weight < MinWeight)
            throw new ValidationException($"Весомость вопроса не может быть меньше {MinWeight}");
    }
}