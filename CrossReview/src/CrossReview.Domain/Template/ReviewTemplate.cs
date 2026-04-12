namespace CrossReview.Domain.Template;

public class ReviewTemplate
{
    private List<ReviewQuestion> _questions;
    
    public ReviewTemplate(Guid id, string title, IEnumerable<ReviewQuestion> questions, bool isActive = true)
    {
        Id = id;
        Title = title;
        _questions = questions.ToList();
        IsActive = isActive;
    }
    
    public Guid Id { get; }
    public string Title { get; }
    public IReadOnlyList<ReviewQuestion> Questions => _questions;
    public bool IsActive { get; } 
}