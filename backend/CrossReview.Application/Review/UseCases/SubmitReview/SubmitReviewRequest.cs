namespace CrossReview.Application.Review.UseCases.SubmitReview;

public record SubmitReviewRequest(Guid ReviewId, Guid TemplateId);
