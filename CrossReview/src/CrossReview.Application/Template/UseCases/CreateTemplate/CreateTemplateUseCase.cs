using CrossReview.Domain.Template;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.CreateTemplate;

public class CreateTemplateUseCase
{
    private readonly ILogger<CreateTemplateUseCase> _logger;
    private readonly IValidator<CreateTemplateRequest> _validator;
    private readonly ITemplateRepository _repository;
    
    public CreateTemplateUseCase(
        ILogger<CreateTemplateUseCase> logger, 
        IValidator<CreateTemplateRequest> validator, 
        ITemplateRepository repository)
    {
        _logger = logger;
        _validator = validator;
        _repository = repository;
    }

    public async Task<Result<Guid, Errors>> Execute(CreateTemplateRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return GeneralErrors.ValueIsInvalid().ToErrors();

        var newTemplate = TemplateEntity.Create(request.ProjectId, request.Title);
        
        var result = await _repository.AddAsync(newTemplate, cancellationToken);

        return result;
    }
}