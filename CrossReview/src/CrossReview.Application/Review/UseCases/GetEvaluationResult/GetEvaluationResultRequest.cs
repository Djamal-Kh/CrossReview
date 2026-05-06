namespace CrossReview.Application.Review.UseCases.GetEvaluationResult;

public record GetEvaluationResultRequest(Guid UserId, Guid ProjectId, Guid PeriodId);