using CrossReview.Application.User.DTO;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.GetUserByEmail;

public class GetUserByEmailUseCase
{
    private readonly ILogger<GetUserByEmailUseCase> _logger;
    private readonly IIdentityService _identityService;

    public GetUserByEmailUseCase(
        ILogger<GetUserByEmailUseCase> logger, 
        IIdentityService identityService)
    {
        _logger = logger;
        _identityService = identityService;
    }

    public async Task<Result<UserDto, Errors>> Execute(GetUserByEmailRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _identityService.GetByEmail(request.Email);

        if (user is null)
            return GeneralErrors.NotFound(request.Email).ToErrors();

        _logger.LogInformation("User with email: {Email} was founded", user.Email);

        return user;
    }
}
