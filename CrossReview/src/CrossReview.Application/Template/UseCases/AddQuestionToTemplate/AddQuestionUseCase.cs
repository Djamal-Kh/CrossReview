using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.AddQuestionToTemplate;

public class AddQuestionUseCase
{
    private readonly ILogger<AddQuestionUseCase> _logger;
    private readonly IValidator<AddQuestionRequest> _validator;
    private readonly ITemplateRepository _repository;
    
    public AddQuestionUseCase(
        ILogger<AddQuestionUseCase> logger, 
        IValidator<AddQuestionRequest> validator, 
        ITemplateRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, Errors>> Execute(AddQuestionRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return GeneralErrors.ValueIsInvalid().ToErrors();

        var template = await _repository.GetByIdAsync(request.TemplateId, cancellationToken);

        if (template is null)
            return GeneralErrors.NotFound(request.TemplateId).ToErrors();

        template.AddQuestion(request.Title, request.Weight);
        
        await _repository.SaveAsync(template, cancellationToken);

        _logger.LogInformation("Напишешь че-нить нормальное");
        
        return template.Id;
    }
}