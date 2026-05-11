using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.DeleteUser;

public class DeleteUserUseCase
{
    private readonly ILogger<DeleteUserUseCase> _logger;
    private readonly IIdentityService _identityService;

    public DeleteUserUseCase(
        ILogger<DeleteUserUseCase> logger, IIdentityService identityService)
    {
        _logger = logger;
        _identityService = identityService;
    }

    public async Task<UnitResult<Errors>> Execute(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var isSuccess = await _identityService.Delete(request.UserId);

        if (!isSuccess)
            return GeneralErrors.NotFound(request.UserId).ToErrors();

        _logger.LogInformation("User {UserId} was deleted", request.UserId);

        return UnitResult.Success<Errors>();
    }
}
