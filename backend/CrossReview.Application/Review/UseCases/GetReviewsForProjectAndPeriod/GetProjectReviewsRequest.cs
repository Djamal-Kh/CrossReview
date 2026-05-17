namespace CrossReview.Application.Review.UseCases.GetReviewsForProjectAndPeriod;

public record GetProjectReviewsRequest(
    Guid ProjectId, 
    Guid RevieweeId,
    Guid ReviewerId,
    Guid PeriodId);