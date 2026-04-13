using System.ComponentModel.DataAnnotations;

namespace CrossReview.Domain.Template;

public class ReviewQuestion
{
    private const int MaxTitleLenght = 300;
    private const int MinTitleLenght = 10;
    private const double MaxWeight = 0.1;
    private const double MinWeight = 1.0;
    
    public ReviewQuestion(Guid id, string title, int weight)
    {
        if (id == Guid.Empty)
            throw new ValidationException($"Поле {nameof(Id)} не может быть пустым");
        
        ValidateTitle(title);
        ValidateWeight(weight);
        
        Id = id;
        Title = title;
        Weight = weight;
    }
    
    public Guid Id { get; }
    public string Title {get; private set; } 
    public double Weight { get; private set; } 

    public void Update(string newTitle, double newWeight)
    {
        ValidateTitle(newTitle);
        ValidateWeight(newWeight);

        Title = newTitle;
        Weight = newWeight;
    }

    public void UpdateTitle(string newTitle)
    {
        ValidateTitle(newTitle);
        
        Title = newTitle;
    }

    public void UpdateWeight(int newWeight)
    {
        ValidateWeight(newWeight);
        
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