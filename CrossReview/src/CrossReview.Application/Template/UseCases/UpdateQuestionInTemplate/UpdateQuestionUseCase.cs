using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.UpdateQuestionInTemplate;

public class UpdateQuestionUseCase
{
    private readonly ILogger<UpdateQuestionUseCase> _logger;
    private readonly IValidator<UpdateQuestionRequest> _validator;
    private readonly ITemplateRepository _templateRepository;
    
    public UpdateQuestionUseCase(
        ILogger<UpdateQuestionUseCase> logger,
        IValidator<UpdateQuestionRequest> validator,
        ITemplateRepository templateRepository)
    {
        _logger = logger;
        _validator = validator;
        _templateRepository = templateRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(UpdateQuestionRequest request, CancellationToken cancellationToken)
    {
        var validatonResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validatonResult.IsValid)
            return GeneralErrors.ValueIsInvalid().ToErrors();

        if (request.Title is null && request.Weight is null)
            return GeneralErrors.ValueIsRequired().ToErrors();
        
        var template = await _templateRepository.GetByIdAsync(request.TemplateId, cancellationToken);
        
        if (template is null)
            return GeneralErrors.NotFound(request.TemplateId).ToErrors();

        var question = template.Questions.FirstOrDefault(q => q.Id == request.TemplateId);

        if (question is null)
            return GeneralErrors.NotFound(request.QuestionId).ToErrors();

        bool weightHasValue = true;
        
        if (request.Weight.Value == 0)
            weightHasValue = false;
        
        if (weightHasValue)
            question.Update(request.Title, request.Weight.Value);
        
        else 
            question.Update(request.Title);
        
        await _templateRepository.SaveAsync(cancellationToken);
        
        _logger.LogInformation("инфа");

        return template.Id;
    }
}