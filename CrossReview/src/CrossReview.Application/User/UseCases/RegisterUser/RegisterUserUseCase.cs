using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.Register;

public class RegisterUserUseCase
{
    private readonly ILogger<RegisterUserUseCase> _logger;
    private readonly IValidator<RegisterUserRequest> _validator;
    private readonly IIdentityService _identityService;
    private readonly IJwtProvider _jwtProvider;

    public RegisterUserUseCase(
        ILogger<RegisterUserUseCase> logger,
        IValidator<RegisterUserRequest> validator,
        IIdentityService identityService,
        IJwtProvider jwtProvider)
    {
        _logger = logger;
        _validator = validator;
        _identityService = identityService;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<string, Errors>> Execute(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        var result = await _identityService.RegisterUser(request);

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
