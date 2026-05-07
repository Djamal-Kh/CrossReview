using CrossReview.Application.Template;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.SubmitReview;

public class SubmitReviewUseCase
{
    private readonly ILogger<SubmitReviewUseCase> _logger;
    private readonly IReviewRepository _reviewRepository;
    private readonly ITemplateRepository _templateRepository;
    
    public SubmitReviewUseCase(
        ILogger<SubmitReviewUseCase> logger,
        IReviewRepository reviewRepository, 
        ITemplateRepository templateRepository)
    {
        _logger = logger;
        _reviewRepository = reviewRepository;
        _templateRepository = templateRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(SubmitReviewRequest request, CancellationToken cancellationToken)
    {
        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);

        if (review is null)
            return GeneralErrors.NotFound(request.ReviewId).ToErrors();

        var template = await _templateRepository.GetByIdAsync(request.TemplateId);

        if (template is null)
            return GeneralErrors.NotFound(request.TemplateId).ToErrors();
        
        var templateQuestionIds = template.Questions.Select(x => x.Id).ToList();
        
        review.Submit(templateQuestionIds);
        
        await _reviewRepository.SaveAsync(cancellationToken);
        
        _logger.LogInformation("инфа");

        return review.Id;
    }
}