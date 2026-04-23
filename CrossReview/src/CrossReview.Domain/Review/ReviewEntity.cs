using System.ComponentModel.DataAnnotations;

namespace CrossReview.Domain.Review;

public class ReviewEntity
{
    private List<ReviewAnswer> _answers;

    private ReviewEntity(
        Guid id, 
        Guid reviewerId, 
        Guid revieweeId, 
        Guid projectId, 
        Guid templateId, 
        Guid periodId)
    {
        Validate(id, reviewerId, revieweeId, projectId, templateId, periodId);
        
        Id = id;
        ReviewerId = reviewerId;
        RevieweeId = revieweeId;
        ProjectId = projectId;
        TemplateId = templateId;
        PeriodId = periodId;
        _answers = [];
        Status = EnumReviewStatus.Draft;
    }
    
    public Guid Id { get; }
    public Guid ReviewerId { get; } 
    public Guid RevieweeId { get; } 
    public Guid ProjectId { get; }
    public Guid TemplateId { get; }
    public IReadOnlyList<ReviewAnswer> Answers => _answers;
    public Guid PeriodId { get; }
    public EnumReviewStatus Status { get; private set; }

    public static ReviewEntity Create(
        Guid reviewerId, 
        Guid revieweeId, 
        Guid projectId, 
        Guid templateId,
        Guid periodId)
    {
        return new ReviewEntity(
            Guid.NewGuid(),
            reviewerId, 
            revieweeId, 
            projectId, 
            templateId,
            periodId);
    }
    
    //todo нужна будет более серьезная бизнес-логика (валидация)
    public bool IsCompleted(IEnumerable<Guid> templateQuestions)
    {
        var questionIds = templateQuestions.ToList();
        
        if (_answers.Count != templateQuestions.Count())
            return false;

        if (questionIds.Except(_answers.Select(q => q.QuestionId)).Any())
            return false;

        return true;
    }

    private void EnsureCompleted(IEnumerable<Guid> templateQuestionIds)
    {
        if (!IsCompleted(templateQuestionIds))
            throw new ValidationException("Ревью еще не заполнено");
    }
    
    public void Submit(IEnumerable<Guid> templateQuestionIds)
    {
        if (!_answers.Any())
            throw new ValidationException("Нельзя опубликовать ревью без ответов");
        
        EnsureEditable();
        
        EnsureCompleted(templateQuestionIds);
        
        Status = EnumReviewStatus.Submitted;
    }

    public void Close()
    {
        if (Status == EnumReviewStatus.Closed)
            return;
        
        if (Status != EnumReviewStatus.Submitted)
            throw new ValidationException("Нельзя закрывать неопубликованные ревью");
        
        Status = EnumReviewStatus.Closed;
    }
    
    public void EnsureEditable()
    {
        if (Status != EnumReviewStatus.Draft)
            throw new ValidationException("Нельзя редактировать ревью");
    }
    
    public void AddAnswer(Guid questionId, int score, string comment)
    {
        EnsureEditable();
        
        var answer = ReviewAnswer.Create(questionId, score, comment);
        
        if (_answers.Any(q => q.QuestionId == answer.QuestionId))
            throw new ValidationException("Ответа для такого вопроса не существует");
        
        _answers.Add(answer);
    }
    
    public void UpdateAnswer(Guid questionId, int score, string comment)
    {
        EnsureEditable();
        
        if (!_answers.Any())
            throw new ValidationException("Нет ни одного ответа");
        
        var answer = _answers.FirstOrDefault(q => q.QuestionId == questionId);

        if (answer is null)
            throw new ValidationException("Ответ не найден");
        
        answer.Update(score, comment);
    }

    public void RemoveAnswer(Guid questionId)
    {
        EnsureEditable();
        
        if (!_answers.Any())
            throw new ValidationException("Нет ни одного ответа");
        
        var answer = _answers.FirstOrDefault(q => q.QuestionId == questionId);
        
        if (answer is null)
            throw new ValidationException("Ответ не найден");
        
        _answers.Remove(answer);
    }
    
    public double CalculateAverageScore()
    {
        if (!_answers.Any())
            throw new ValidationException("Передан пустой список вопросов");
        
        var score = _answers.Average(q => q.Score);
        return score;
    }
    
    private void Validate(Guid id, Guid reviewerId, Guid revieweeId, Guid projectId, Guid templateId, Guid periodId)
    {
        if (id == Guid.Empty)
            throw new ValidationException($"Поле {nameof(Id)} не может быть пустым");
        
        if (reviewerId == Guid.Empty)
            throw new ValidationException($"Поле {nameof(ReviewerId)} не может быть пустым");
        
        if (revieweeId == Guid.Empty)
            throw new ValidationException($"Поле {nameof(RevieweeId)} не может быть пустым");
        
        if (revieweeId == reviewerId)
            throw new ValidationException("Нельзя оценивать самого себя");
        
        if (projectId == Guid.Empty)
            throw new ValidationException($"Поле {nameof(ProjectId)} не может быть пустым");
        
        if (templateId == Guid.Empty)
            throw new ValidationException($"Поле {nameof(TemplateId)} не может быть пустым");
        
        if (periodId == Guid.Empty)
            throw new ValidationException($"Поле {nameof(PeriodId)} не может быть пустым");
    }
}