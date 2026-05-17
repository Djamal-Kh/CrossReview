using System.Security.Principal;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.Login;

public class LoginUseCase
{
    private readonly ILogger<LoginUseCase> _logger;
    private readonly IJwtProvider _jwtProvider;
    private readonly IIdentityService _identityServices;

    public LoginUseCase(
        ILogger<LoginUseCase> logger,
        IJwtProvider jwtProvider, 
        IIdentityService identityServices)
    {
        _logger = logger;
        _jwtProvider = jwtProvider;
        _identityServices = identityServices;
    }

    public async Task<Result<string, Errors>> Execute(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        var jwtUser = await _identityServices.Login(request.Email, request.Password);

        if (jwtUser is null)
            return GeneralErrors.ValueIsInvalid("credentials").ToErrors();

        var token = _jwtProvider.Generate(jwtUser);

        return token;
    }
}
