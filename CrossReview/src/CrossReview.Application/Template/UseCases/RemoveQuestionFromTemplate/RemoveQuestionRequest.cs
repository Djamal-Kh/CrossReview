namespace CrossReview.Application.Template.UseCases.RemoveQuestionFromTemplate;

public record RemoveQuestionRequest(Guid TemplateId, Guid QuestionId);