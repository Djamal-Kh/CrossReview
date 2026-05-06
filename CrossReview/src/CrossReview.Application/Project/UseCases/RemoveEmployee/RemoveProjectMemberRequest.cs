namespace CrossReview.Application.Project.UseCases.RemoveEmployee;

public record RemoveProjectMemberRequest(Guid ProjectId, Guid UserId);