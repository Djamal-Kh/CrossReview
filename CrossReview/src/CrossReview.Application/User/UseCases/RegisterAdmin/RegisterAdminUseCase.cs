using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.RegisterAdmin;

public class RegisterAdminUseCase
{
    private readonly ILogger<RegisterAdminUseCase> _logger;
    private readonly IValidator<RegisterAdminRequest> _validator;
    private readonly IIdentityService _identityService;
    private readonly IJwtProvider _jwtProvider;
    
    public RegisterAdminUseCase(
        ILogger<RegisterAdminUseCase> logger,
        IValidator<RegisterAdminRequest> validator, 
        IIdentityService identityService,
        IJwtProvider jwtProvider)
    {
        _logger = logger;
        _validator = validator;
        _identityService = identityService;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<string, Errors>> Execute(RegisterAdminRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            return validationResult.ToList();

        var result = await _identityService.RegisterAdmin(request);
        
        if (result.IsFailure)
            return GeneralErrors.Failure("User creation failed").ToErrors();
        
        var user = result.Value;
        
        var jwtUser = new JwtUserModel{
            Id = user.Id,
            Email = user.Email,
            Roles = user.Roles
        };
        
        var token = _jwtProvider.Generate(jwtUser);

        return token;
    }
}