using CrossReview.Domain.Template;
using CSharpFunctionalExtensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.Template.UseCases.GetTemplateById;

public class GetTemplateByIdUseCase
{
    private readonly ITemplateRepository _templateRepository;
    
    public GetTemplateByIdUseCase(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }

    public async Task<Result<TemplateEntity, Errors>> Execute(GetTemplateByIdRequest request,
        CancellationToken cancellationToken)
    {
        var template = await _templateRepository.GetByIdAsync(request.TemplateId);

        if (template is null)
            return GeneralErrors.NotFound(request.TemplateId).ToErrors();

        return template;
    }
}