using CrossReview.Domain.User;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared.Common.Extensions;
using Shared.Common.ResultPattern;

namespace CrossReview.Application.User.UseCases.CreateUser;

public class CreateUserUseCase
{
    private readonly ILogger<CreateUserUseCase> _logger;
    private readonly IValidator<CreateUserRequest> _validator;
    private readonly IUserRepository _userRepository;

    public CreateUserUseCase(
        ILogger<CreateUserUseCase> logger,
        IValidator<CreateUserRequest> validator,
        IUserRepository userRepository)
    {
        _logger = logger;
        _validator = validator;
        _userRepository = userRepository;
    }

    public async Task<Result<Guid, Errors>> Execute(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        //todo захэшировать пароль. Реализуй когда будешь работать с аутентификацией и авторизацией

        var isEmailExist = await _userRepository.ExistByEmailAsync(request.Email);

        if (isEmailExist)
            return GeneralErrors.ValueAlreadyExists(request.Email).ToErrors();
        
        var user = UserEntity.Create(request.FirstName, request.LastName, request.Password, request.Email,
            request.PhoneNumber, request.Role);
        
        var result = await _userRepository.AddAsync(user, cancellationToken);

        if (result.IsFailure)
        {
            _logger.LogWarning("Failed to addUser: {email}", request.Email);
            return GeneralErrors.Failure().ToErrors();
        }
        
        _logger.LogInformation("Project with email {Email} was added", request.Email);

        return result.Value;
    }
}