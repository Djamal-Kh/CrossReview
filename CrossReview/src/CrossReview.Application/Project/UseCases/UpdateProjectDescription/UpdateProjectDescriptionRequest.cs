namespace CrossReview.Application.Project.UseCases.UpdateProjectDescription;

public record UpdateProjectDescriptionRequest(Guid ProjectId, string Description);