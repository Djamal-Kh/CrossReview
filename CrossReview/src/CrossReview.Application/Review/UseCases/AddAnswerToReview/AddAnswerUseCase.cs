using CrossReview.Application.Template;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Review.UseCases.AddAnswerToReview;

public class AddAnswerUseCase
{
    private readonly ILogger<AddAnswerUseCase> _logger;
    private readonly IValidator<AddAnswerRequest> _validator;
    private readonly IReviewRepository _reviewRepository;
    private readonly ITemplateRepository _templateRepository;
    
    public AddAnswerUseCase(
        ILogger<AddAnswerUseCase> logger, 
        IValidator<AddAnswerRequest> validator, 
        IReviewRepository reviewRepository, 
        ITemplateRepository templateRepository)
    {
        _logger = logger;
        _validator = validator;
        _reviewRepository = reviewRepository;
        _templateRepository = templateRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(AddAnswerRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return GeneralErrors.ValueIsInvalid().ToErrors();

        var review = await _reviewRepository.GetByIdAsync(request.ReviewId, cancellationToken);

        if (review is null)
            return GeneralErrors.NotFound(request.ReviewId).ToErrors();

        var template = await _templateRepository.GetByIdAsync(review.TemplateId, cancellationToken);

        if (template is null)
            return GeneralErrors.NotFound(review.TemplateId).ToErrors();
        
        bool isQuestionExist = template.HasQuestion(request.QuestionId);
        
        if (isQuestionExist is false)
            return GeneralErrors.NotFound(request.QuestionId).ToErrors();
        
        review.AddAnswer(request.QuestionId, request.Score, request.Comment);
        
        await _reviewRepository.SaveAsync(review, cancellationToken);

        _logger.LogInformation("здесь будет инфа");
        
        return review.Id;
    }
}