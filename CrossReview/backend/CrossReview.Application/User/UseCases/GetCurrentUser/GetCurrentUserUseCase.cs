using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CrossReview.Application.User.DTO;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.GetCurrentUser;

public class GetCurrentUserUseCase
{
    private readonly ILogger<GetCurrentUserUseCase> _logger;
    private readonly IIdentityService _identityService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetCurrentUserUseCase(
        ILogger<GetCurrentUserUseCase> logger, 
        IIdentityService identityService,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _identityService = identityService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<UserDto, Errors>> Execute(CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                     ?? _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            _logger.LogWarning("Unauthorized access attempt to /api/user/me");
            return GeneralErrors.Failure("message").ToErrors();
        }

        var user = await _identityService.GetById(userGuid);

        if (user is null)
        {
            _logger.LogWarning("User with ID: {UserId} not found", userGuid);
            return GeneralErrors.NotFound($"User with ID: {userGuid}").ToErrors();
        }

        _logger.LogInformation("Current user with ID: {UserId} was found", user.Id);

        return user;
    }
}