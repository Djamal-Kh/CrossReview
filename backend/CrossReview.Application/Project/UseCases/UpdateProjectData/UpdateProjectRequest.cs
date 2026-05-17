namespace CrossReview.Application.Project.UseCases.UpdateProjectData;

public record UpdateProjectRequest(Guid ProjectId, string Title, string Description);