namespace CrossReview.Application.Review.UseCases.GetReviewsForUser;

public record GetReviewsForUserRequest(Guid UserId, Guid? ProjectId, Guid? PeriodId);