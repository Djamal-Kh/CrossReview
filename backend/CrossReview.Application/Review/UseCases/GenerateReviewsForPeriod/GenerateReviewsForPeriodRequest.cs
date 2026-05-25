namespace CrossReview.Application.Review.UseCases.GenerateReviewsForPeriod;

public record GenerateReviewsForPeriodRequest(Guid ProjectId,
    Guid PeriodId,
    Guid TemplateId,
    Guid RequestedByUserId,
    bool IsAdmin);