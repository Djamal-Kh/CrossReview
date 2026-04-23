namespace CrossReview.Application.Template.UseCases.UpdateQuestionInTemplate;

public record UpdateQuestionRequest(Guid TemplateId, Guid QuestionId, string? Title, double? Weight);