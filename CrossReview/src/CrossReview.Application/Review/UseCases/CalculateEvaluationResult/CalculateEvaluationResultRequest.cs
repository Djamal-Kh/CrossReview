namespace CrossReview.Application.Review.UseCases.CalculateEvaluationResult;

public record CalculateEvaluationResultRequest(Guid UserId, Guid ProjectId, Guid PeriodId);