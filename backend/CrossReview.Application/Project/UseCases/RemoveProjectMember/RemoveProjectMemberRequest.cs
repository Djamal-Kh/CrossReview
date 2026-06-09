namespace CrossReview.Application.Project.UseCases.RemoveProjectMember;

public record RemoveProjectMemberRequest(Guid ProjectId, Guid UserId, Guid RequestedByUserId, bool IsAdmin);