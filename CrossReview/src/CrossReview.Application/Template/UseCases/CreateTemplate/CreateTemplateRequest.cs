using CrossReview.Domain.Template;

namespace CrossReview.Application.Template.UseCases.CreateTemplate;

public record CreateTemplateRequest(Guid ProjectId, string Title);