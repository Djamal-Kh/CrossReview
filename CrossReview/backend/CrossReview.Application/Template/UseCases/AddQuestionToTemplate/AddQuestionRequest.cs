namespace CrossReview.Application.Template.UseCases.AddQuestionToTemplate;

public record AddQuestionRequest(Guid TemplateId, string Title, double Weight);