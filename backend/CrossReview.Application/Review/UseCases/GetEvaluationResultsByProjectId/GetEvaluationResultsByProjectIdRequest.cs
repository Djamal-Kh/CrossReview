namespace CrossReview.Application.Review.UseCases.GetEvaluationResultsByProjectId;

public record GetEvaluationResultsByProjectIdRequest(Guid? UserId, Guid ProjectId, bool IsAdmin);