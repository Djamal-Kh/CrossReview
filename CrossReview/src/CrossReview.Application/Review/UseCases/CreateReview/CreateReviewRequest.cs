namespace CrossReview.Application.Review.UseCases.CreateReview;

public record CreateReviewRequest(
    Guid ReviewerId, 
    Guid RevieweeId, 
    Guid ProjectId,
    Guid TemplateId,
    Guid PeriodId);