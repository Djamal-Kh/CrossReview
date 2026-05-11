namespace CrossReview.Application.User.UseCases.CreateUser__Register_;

public record RegisterUserRequest(string FirstName, string LastName, string Email, string Password, string PhoneNumber);