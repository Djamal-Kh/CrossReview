namespace CrossReview.Application.Project.UseCases.GetProjectMemberById;

public record GetProjectMemberByIdRequest(Guid ProjectId, Guid UserId);