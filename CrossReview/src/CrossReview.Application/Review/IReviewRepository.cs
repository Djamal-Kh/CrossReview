using CrossReview.Application.Review.UseCases.CreateReview;
using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review;

public interface IReviewRepository
{
    public Task<Result<Guid, Error>> AddAsync(ReviewEntity review, CancellationToken cancellationToken = default);
    public Task SaveAsync(ReviewEntity review, CancellationToken cancellationToken = default);
    public Task<ReviewEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<List<ReviewEntity?>> GetByReviewee(Guid userId, Guid projectId, Guid periodId,
        CancellationToken cancellationToken = default);

    public Task<ReviewEntity?> GetByProject(Guid projectId, Guid revieweeId, Guid reviewerId, Guid periodId,
        CancellationToken cancellationToken = default);
    
    public Task<List<ReviewEntity>> GetAllAsync(Guid userId, Guid projectId, Guid periodId, CancellationToken cancellationToken = default);
    public Task<Guid?> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<bool> ExistsByTitleAsync(string title, CancellationToken cancellationToken = default);
    public Task<bool> ExistsReviewAsync(Guid reviewerId,Guid revieweeId, Guid periodId, CancellationToken cancellationToken = default);
}