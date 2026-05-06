namespace CrossReview.Application.Review.UseCases.RemoveAnswerFromReview;

public record RemoveAnswerRequest(Guid ReviewId, Guid QuestionId);