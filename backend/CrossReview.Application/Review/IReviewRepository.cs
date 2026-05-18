using CrossReview.Application.Review.UseCases.CreateReview;
using CrossReview.Domain.Review;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review;

public interface IReviewRepository
{
    public Task<Guid> AddAsync(ReviewEntity review, CancellationToken cancellationToken = default);
    public Task SaveAsync(CancellationToken cancellationToken = default);
    public Task<ReviewEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<List<ReviewEntity?>> GetByReviewee(Guid userId, Guid projectId, Guid periodId,
        CancellationToken cancellationToken = default);

    public Task<List<ReviewEntity>> GetByProject(Guid? projectId, Guid? revieweeId, Guid? reviewerId, Guid? periodId,
        CancellationToken cancellationToken = default);
    
    public Task<List<ReviewEntity>> GetAllAsync(Guid userId, Guid? projectId, Guid? periodId, CancellationToken cancellationToken = default);
    public Task<List<ReviewEntity>> GetAllAsyncForSeed(Guid prijectId, CancellationToken cancellationToken = default);
    public Task<Guid?> DeleteAsync(ReviewEntity review, CancellationToken cancellationToken = default);
    public Task<bool> ExistsReviewAsync(Guid reviewerId,Guid revieweeId, Guid periodId, CancellationToken cancellationToken = default);
}