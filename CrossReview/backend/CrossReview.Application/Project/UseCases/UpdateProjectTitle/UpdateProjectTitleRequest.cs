namespace CrossReview.Application.Project.UseCases.UpdateProjectTitle;

public record UpdateProjectTitleRequest(Guid ProjectId, string Title);