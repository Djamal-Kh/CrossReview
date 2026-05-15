namespace CrossReview.Application.User.UseCases.RegisterUser;

public record RegisterUserRequest(string FirstName, string LastName, string Email, string Password, string PhoneNumber);